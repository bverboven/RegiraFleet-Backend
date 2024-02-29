using Microsoft.EntityFrameworkCore;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionRepository(FleetContext dbContext) : FleetRepository<Intervention, InterventionSearchObject, EntitySortBy, InterventionIncludes>(dbContext)
{
    public override IQueryable<Intervention> Filter(IQueryable<Intervention> query, InterventionSearchObject? so)
    {
        query = base.Filter(query, so);

        if (so?.CarId?.Any() == true)
        {
            query = query.Where(x => so.CarId.Contains(x.VehicleId!.Value));
        }
        if (so?.SupplierId?.Any() == true)
        {
            query = query.Where(x => so.SupplierId.Contains(x.SupplierId!.Value));
        }
        if (so?.InterventionTypeId?.Any() == true)
        {
            query = query.Where(x => x.InterventionTypes!.Any(it => so.InterventionTypeId.Contains(it.Id)));
        }

        return query;
    }
    public override IQueryable<Intervention> SortBy(IQueryable<Intervention> query, EntitySortBy? sortBy = null)
    {
        return query
            .OrderByDescending(x => x.Invoice!.InvoiceDate)
            .ThenByDescending(x => x.Id);
    }
    public override IQueryable<Intervention> AddIncludes(IQueryable<Intervention> query, InterventionIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(InterventionIncludes.Vehicles))
            {
                query = query
                    .Include(x => x.Vehicle!)
                    .ThenInclude(c => c.VehicleType)
                    .Include(x => x.Vehicle!)
                    .ThenInclude(c => c.Brand);
            }
            if (includes.Value.HasFlag(InterventionIncludes.Suppliers))
            {
                query = query
                    .Include(x => x.Supplier!)
                    .ThenInclude(s => s.ContactData);
            }
            if (includes.Value.HasFlag(InterventionIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes);
            }
            if (includes.Value.HasFlag(InterventionIncludes.Attachments))
            {
                query = query
                    .Include(x => x.Attachments);
            }
        }

        return query;
    }
}