using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<Operator, OperatorSearchObject, EntitySortBy, OperatorIncludes>(dbContext, appContext)
{
    public override IQueryable<Operator> Filter(IQueryable<Operator> query, OperatorSearchObject? so)
    {
        query = base.Filter(query, so);
        if (so != null)
        {
            var qHelper = QKeywordHelper.Create();

            query = query.FilterArchivable(so.IsArchived);
            query = query.FilterCode(so.Code);
            query = query.FilterTitle(qHelper.Parse(so.Title));

            if (!string.IsNullOrWhiteSpace(so.IdentificationNumber))
            {
                query = query.Where(x => x.IdentificationNumber!.Equals(so.IdentificationNumber));
            }

            if (!string.IsNullOrWhiteSpace(so.Phone))
            {
                query = query.Where(x => x.ContactData!.Any(cd => cd.Value == so.Phone));
            }

            if (so.InterventionTypeId?.Any() == true)
            {
                query = query.Where(x => so.InterventionTypeId.All(id => x.InterventionTypes!.Any(ot => ot.InterventionTypeId == id)));
            }
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
                query = query
                    .Include(x => x.ContactData!.OrderBy(a => a.SortOrder));
            }
            if (includes.Value.HasFlag(OperatorIncludes.Addresses))
            {
                query = query
                    .Include(x => x.Addresses!.OrderBy(a => a.SortOrder));
            }
            if (includes.Value.HasFlag(OperatorIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes!)
                    .ThenInclude(x => x.InterventionType);
            }
            // Attachments
            if (includes.Value.HasFlag(OperatorIncludes.Attachments))
            {
                query = query.Include(x => x.Attachments!)
                    .ThenInclude(a => a.Attachment);
            }
        }

        return query;
    }

    public override void Modify(Operator item, Operator original)
    {
        DbContext.UpdateEntityChildCollection(original, item, x => x.Addresses, (x, collection) => x.Addresses = collection);
        DbContext.UpdateEntityChildCollection(original, item, x => x.ContactData, (x, collection) => x.ContactData = collection);

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

        if (item.Attachments != null)
        {
            DbContext.ModifyEntityAttachments(original, item);
        }

        base.Modify(item, original);
    }
    public override void PrepareItem(Operator item)
    {
        item.Addresses?.Prepare();
        item.ContactData?.Prepare();

        base.PrepareItem(item);
    }
}