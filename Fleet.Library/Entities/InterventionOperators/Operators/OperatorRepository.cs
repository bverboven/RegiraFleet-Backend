using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorRepository(FleetContextBase dbContext, IFleetAppContext appContext, ContactDataNormalizer contactDataNormalizer) : FleetRepositoryBase<Operator, OperatorSearchObject, EntitySortBy, OperatorIncludes>(dbContext, appContext)
{
    public override IQueryable<Operator> Filter(IQueryable<Operator> query, OperatorSearchObject? so)
    {
        query = base.Filter(query, so);
        if (so != null)
        {
            var qHelper = QKeywordHelper.Create();

            // Code
            query = query.FilterCode(so.Code);

            // IdentificationNumber
            if (!string.IsNullOrWhiteSpace(so.IdentificationNumber))
            {
                query = query.Where(x => x.IdentificationNumber!.Equals(so.IdentificationNumber));
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var keywords = qHelper.Parse(so.Title);
                foreach (var kw in keywords)
                {
                    query = query.Where(x => x.Code == so.Title || dbContext.ILike(x.Title, kw.Q!));
                }
            }
            // Phone
            if (!string.IsNullOrWhiteSpace(so.Phone))
            {
                var q = contactDataNormalizer.Normalize(so.Phone, ContactDataTypes.Phone);
                query = query.Where(x => x.ContactData!.Any(cd => cd.DataType == ContactDataTypes.Phone && dbContext.ILike(cd.NormalizedValue!, $"%{q}%")));
            }
            // Email
            if (!string.IsNullOrWhiteSpace(so.Email))
            {
                var q = contactDataNormalizer.Normalize(so.Email, ContactDataTypes.Email);
                query = query.Where(x => x.ContactData!.Any(cd => cd.DataType == ContactDataTypes.Email && dbContext.ILike(cd.NormalizedValue!, $"%{q}%")));
            }
            // Address
            if (!string.IsNullOrWhiteSpace(so.Address))
            {
                var keywords = qHelper.Parse(so.Address);
                foreach (var kw in keywords)
                {
                    query = query.Where(x => x.Addresses!.Any(a => dbContext.ILike(a.NormalizedContent!, kw.QW!)));
                }
            }
            // InterventionTypeId
            if (so.InterventionTypeId?.Any() == true)
            {
                query = query.Where(x => so.InterventionTypeId.All(id => x.InterventionTypes!.Any(ot => ot.InterventionTypeId == id)));
            }
            // HasIntervention
            if (so.HasIntervention.HasValue)
            {
                query = query.Where(x => DbContext.Interventions.Any(i => i.OperatorId == x.Id));
            }
            // Q
            query = dbContext.FilterILikeQ(query, qHelper.Parse(so.Q));
        }

        return query;
    }
    public override IQueryable<Operator> SortBy(IQueryable<Operator> query, EntitySortBy? sortBy = null)
    {
        return query.OrderBy(x => x.NormalizedTitle);
    }
    public override IQueryable<Operator> AddIncludes(IQueryable<Operator> query, OperatorIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(OperatorIncludes.ContactData))
            {
                query = query.Include(x => x.ContactData!.OrderBy(a => a.SortOrder));
            }
            if (includes.Value.HasFlag(OperatorIncludes.Addresses))
            {
                query = query.Include(x => x.Addresses!.OrderBy(a => a.SortOrder));
            }
            if (includes.Value.HasFlag(OperatorIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes!)
                    .ThenInclude(x => x.InterventionType);
            }
            // Labels
            if (includes.Value.HasFlag(OperatorIncludes.Labels))
            {
                query = query.Include(x => x.Labels!.OrderBy(a => a.SortOrder));
            }
            // Attachments
            if (includes.Value.HasFlag(OperatorIncludes.Attachments))
            {
                query = query
                    .Include(x => x.Attachments!)
                    .ThenInclude(a => a.Attachment);
            }
        }

        return query;
    }

    public override void Modify(Operator item, Operator original)
    {
        // Addresses
        DbContext.UpdateEntityChildCollection(original, item, x => x.Addresses, (x, collection) => x.Addresses = collection);
        // Contact Data
        DbContext.UpdateEntityChildCollection(original, item, x => x.ContactData, (x, collection) => x.ContactData = collection);
        // Labels
        DbContext.UpdateEntityChildCollection(original, item, x => x.Labels, (x, collection) => x.Labels = collection);

        // Intervent Types
        if (item.InterventionTypes != null)
        {
            var itemsToRemove = original.InterventionTypes?
                .Where(o => item.InterventionTypes.All(x => o.InterventionTypeId != x.InterventionTypeId))
                .ToArray() ?? Array.Empty<OperatorInterventionType>();
            var itemsToAdd = item.InterventionTypes
                .Where(x => original.InterventionTypes == null || original.InterventionTypes.All(o => x.InterventionTypeId != o.InterventionTypeId))
                .ToArray();
            foreach (var itemToRemove in itemsToRemove)
            {
                DbContext.Entry(itemToRemove).State = EntityState.Deleted;
            }
            foreach (var itemToAdd in itemsToAdd)
            {
                DbContext.Entry(itemToAdd).State = EntityState.Added;
            }
            original.InterventionTypes = (original.InterventionTypes ?? Array.Empty<OperatorInterventionType>())
                .Except(itemsToRemove)
                .Concat(itemsToAdd)
                .ToList();
        }        
        // Attachments
        if (item.Attachments != null)
        {
            DbContext.ModifyEntityAttachments(original, item);
        }

        base.Modify(item, original);
    }
    public override void PrepareItem(Operator item)
    {
        base.PrepareItem(item);

        item.Addresses?.Prepare();
        item.ContactData?.Prepare();
        item.Labels?.Prepare();
    }
}