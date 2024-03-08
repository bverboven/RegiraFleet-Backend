using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;

namespace Regira.Fleet.Entities.Vehicles.VehicleTypes;

public class VehicleTypeRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<VehicleType, VehicleTypeSearchObject>(dbContext, appContext)
{
    public override IQueryable<VehicleType> Filter(IQueryable<VehicleType> query, VehicleTypeSearchObject? so)
    {
        query = base.Filter(query, so);
        if (so != null)
        {
            var qHelper = QKeywordHelper.Create();

            // Code
            query = query.FilterCode(so.Code);
            // Title
            query = query.FilterILikeTitle(qHelper.Parse(so.Title));
            // Q
            query = query.FilterILikeTitleQ(qHelper.Parse(so.Q));
        }
        return query;
    }
}