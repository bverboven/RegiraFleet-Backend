using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorQueryBuilder(FleetContextBase dbContext, IQKeywordHelper qHelper, ContactDataNormalizer contactDataNormalizer,
    IEnumerable<IGlobalFilteredQueryBuilder> globalFilters,
    IEnumerable<IFilteredQueryBuilder<Operator, OperatorSearchObject>>? filters = null)
    : QueryBuilder<Operator, OperatorSearchObject, EntitySortBy, OperatorIncludes>(globalFilters, filters)
{
    public override IQueryable<Operator> Filter(IQueryable<Operator> query, OperatorSearchObject? so)
    {
        if (so != null)
        {
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
                    query = query.Where(x => x.Code == so.Title || EF.Functions.Like(x.Title, kw.Q!));
                }
            }
            // Phone
            if (!string.IsNullOrWhiteSpace(so.Phone))
            {
                var q = contactDataNormalizer.Normalize(so.Phone, ContactDataTypes.Phone);
                query = query.Where(x => x.ContactData!.Any(cd => cd.DataType == ContactDataTypes.Phone && EF.Functions.Like(cd.NormalizedValue!, $"%{q}%")));
            }
            // Email
            if (!string.IsNullOrWhiteSpace(so.Email))
            {
                var q = contactDataNormalizer.Normalize(so.Email, ContactDataTypes.Email);
                query = query.Where(x => x.ContactData!.Any(cd => cd.DataType == ContactDataTypes.Email && EF.Functions.Like(cd.NormalizedValue!, $"%{q}%")));
            }
            // Address
            if (!string.IsNullOrWhiteSpace(so.Address))
            {
                var keywords = qHelper.Parse(so.Address);
                foreach (var kw in keywords)
                {
                    query = query.Where(x => x.Addresses!.Any(a => EF.Functions.Like(a.NormalizedContent!, kw.QW!)));
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
                query = query.Where(x => dbContext.Interventions.Any(i => i.OperatorId == x.Id));
            }
            // Q
            query = query.FilterILikeQ(qHelper.Parse(so.Q));
        }

        return query;
    }
    public override IQueryable<Operator> SortBy(IQueryable<Operator> query, IList<OperatorSearchObject?>? so, EntitySortBy? sortBy, OperatorIncludes? includes)
        => query.OrderBy(x => x.NormalizedTitle);
    public override IQueryable<Operator> AddIncludes(IQueryable<Operator> query, IList<OperatorSearchObject?>? so, IList<EntitySortBy>? sortByList, OperatorIncludes? includes)
    {
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
}