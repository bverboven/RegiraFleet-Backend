using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorQueryBuilder(IEnumerable<IGlobalFilteredQueryBuilder> globalFilters,
    IEnumerable<IFilteredQueryBuilder<Operator, int, OperatorSearchObject>>? filters = null)
    : QueryBuilder<Operator, OperatorSearchObject, EntitySortBy, OperatorIncludes>(globalFilters, filters)
{
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