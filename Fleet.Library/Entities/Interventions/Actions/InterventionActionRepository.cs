using Microsoft.EntityFrameworkCore;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.Interventions.Actions;

public class InterventionActionRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<InterventionAction, InterventionActionSearchObject, EntitySortBy, InterventionActionIncludes>(dbContext, appContext)
{
    public override IQueryable<InterventionAction> Filter(IQueryable<InterventionAction> query, InterventionActionSearchObject? so)
    {
        query = base.Filter(query, so);

        if (so?.CarId?.Any() == true)
        {
            query = query.Where(x => so.CarId.Contains(x.VehicleId));
        }
        if (so?.OperatorId?.Any() == true)
        {
            query = query.Where(x => so.OperatorId.Contains(x.OperatorId));
        }
        if (so?.InterventionTypeId?.Any() == true)
        {
            query = query.Where(x => x.InterventionTypes!.Any(it => so.InterventionTypeId.Contains(it.Id)));
        }

        return query;
    }
    public override IQueryable<InterventionAction> SortBy(IQueryable<InterventionAction> query, EntitySortBy? sortBy = null)
    {
        return query
            .OrderByDescending(x => x.InterventionDate ?? x.Created)
            //.OrderByDescending(x => x.Invoices!.Max(i => i.InvoiceDate))
            .ThenByDescending(x => x.Id);
    }
    public override IQueryable<InterventionAction> AddIncludes(IQueryable<InterventionAction> query, InterventionActionIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(InterventionActionIncludes.Invoices))
            {
                query = query
                    .Include(x => x.Invoices);
            }
            if (includes.Value.HasFlag(InterventionActionIncludes.Vehicles))
            {
                query = query
                    .Include(x => x.Vehicle!)
                    .ThenInclude(c => c.VehicleType)
                    .Include(x => x.Vehicle!)
                    .ThenInclude(c => c.Brand);
            }
            if (includes.Value.HasFlag(InterventionActionIncludes.Operators))
            {
                query = query
                    .Include(x => x.Operator!)
                    .ThenInclude(s => s.ContactData)
                    .Include(x => x.Operator!)
                    .ThenInclude(s => s.Addresses);
            }
            if (includes.Value.HasFlag(InterventionActionIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes);
            }
            if (includes.Value.HasFlag(InterventionActionIncludes.Attachments))
            {
                query = query
                    .Include(x => x.Attachments);
            }
        }

        return query;
    }
}