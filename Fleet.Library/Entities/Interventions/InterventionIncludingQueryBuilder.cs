using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Fleet.Models.Interventions;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionIncludingQueryBuilder : IIncludableQueryBuilder<Intervention, int, InterventionIncludes>
{
    public IQueryable<Intervention> AddIncludes(IQueryable<Intervention> query, InterventionIncludes? includes = null)
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
                query = query.Include(x => x.Attachments!.OrderBy(a => a.SortOrder))
                    .ThenInclude(a => a.Attachment);
            }
        }

        return query;
    }
}
