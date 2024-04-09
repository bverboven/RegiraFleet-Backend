using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Entities.InterventionTypes;

public class InterventionTypeRepository(FleetContextBase dbContext, IFleetAppContext appContext) : FleetRepositoryBase<InterventionType, InterventionTypeSearchObject>(dbContext, appContext)
{
    public override IQueryable<InterventionType> Filter(IQueryable<InterventionType> query, InterventionTypeSearchObject? so)
    {
        query = base.Filter(query, so);
        if (so != null)
        {
            var qHelper = QKeywordHelper.Create();

            // Code
            query = query.FilterCode(so.Code);
            // Title
            query = dbContext.FilterILikeTitle(query, qHelper.Parse(so.Title));
            // Q
            query = dbContext.FilterILikeTitleQ(query, qHelper.Parse(so.Q));

            // Operator
            if (so.OperatorId?.Any() == true)
            {
                query = query.Where(x => DbContext.InterventionOperators
                    .Where(o => so.OperatorId.Contains(o.Id))
                    .Any(o => o.InterventionTypes!.Any(ot => ot.InterventionTypeId == x.Id))
                );
            }
            // Vehicle
            if (so.VehicleId?.Any() == true)
            {
                query = query.Where(x => DbContext.Vehicles
                    .Where(o => so.VehicleId.Contains(o.Id))
                    .Any(o => o.InterventionTypes!.Any(ot => ot.InterventionTypeId == x.Id))
                );
            }
        }
        return query;
    }

    public override IQueryable<InterventionType> SortBy(IQueryable<InterventionType> query, EntitySortBy? sortBy = null)
    {
        return query.OrderBy(x => x.Title);
    }
}