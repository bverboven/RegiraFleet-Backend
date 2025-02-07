using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Fleet.Models.Interventions;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionQueryBuilder(
    IEnumerable<IGlobalFilteredQueryBuilder> globalFilters,
    IEnumerable<IFilteredQueryBuilder<Intervention, InterventionSearchObject>>? filters = null)
    : QueryBuilder<Intervention, InterventionSearchObject, InterventionSortBy, InterventionIncludes>(globalFilters, filters)
{
    public override IQueryable<Intervention> SortBy(IQueryable<Intervention> query, IList<InterventionSearchObject?>? so, InterventionSortBy? sortBy, InterventionIncludes? includes)
        => query
            .OrderByDescending(x => x.InterventionDate ?? x.Created)
            //.OrderByDescending(x => x.Invoices!.Max(i => i.InvoiceDate))
            .ThenByDescending(x => x.Id);
    public override IQueryable<Intervention> AddIncludes(IQueryable<Intervention> query, IList<InterventionSearchObject?>? so, IList<InterventionSortBy>? sortByList, InterventionIncludes? includes)
    {
        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(InterventionIncludes.Invoice))
            {
                query = query
                    .Include(x => x.Invoice);
            }
            if (includes.Value.HasFlag(InterventionIncludes.Vehicle))
            {
                query = query
                    .Include(x => x.Vehicle!)
                    .ThenInclude(c => c.VehicleType)
                    .Include(x => x.Vehicle!)
                    .ThenInclude(c => c.Brand);
            }
            if (includes.Value.HasFlag(InterventionIncludes.Operator))
            {
                query = query
                    .Include(x => x.Operator!)
                    .ThenInclude(s => s.ContactData)
                    .Include(x => x.Operator!)
                    .ThenInclude(s => s.Addresses);
            }
            if (includes.Value.HasFlag(InterventionIncludes.InterventionType))
            {
                query = query
                    .Include(x => x.InterventionType);
            }
            // Labels
            if (includes.Value.HasFlag(InterventionIncludes.Labels))
            {
                query = query.Include(x => x.Labels!.OrderBy(a => a.SortOrder));
            }
            // Attachments
            if (includes.Value.HasFlag(InterventionIncludes.Attachments))
            {
                query = query.Include(x => x.Attachments!)
                    .ThenInclude(a => a.Attachment);
            }
        }

        return query;
    }
}