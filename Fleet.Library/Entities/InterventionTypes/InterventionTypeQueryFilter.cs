using Regira.Entities.EFcore.Extensions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Entities.InterventionTypes;

public class InterventionTypeQueryFilter(FleetContextBase dbContext)
    : FilteredQueryBuilderBase<InterventionType, InterventionTypeSearchObject>
{
    public override IQueryable<InterventionType> Build(IQueryable<InterventionType> query, InterventionTypeSearchObject? so)
    {
        if (so != null)
        {
            // Code
            query = query.FilterCode(so.Code);

            // Operator
            if (so.OperatorId?.Any() == true)
            {
                query = query.Where(x => dbContext.InterventionOperators
                    .Where(o => so.OperatorId.Contains(o.Id))
                    .Any(o => o.InterventionTypes!.Any(ot => ot.InterventionTypeId == x.Id))
                );
            }
            // Vehicle
            if (so.VehicleId?.Any() == true)
            {
                query = query.Where(x => dbContext.Vehicles
                    .Where(o => so.VehicleId.Contains(o.Id))
                    .Any(o => o.InterventionTypes!.Any(ot => ot.InterventionTypeId == x.Id))
                );
            }
        }

        return query;
    }
}