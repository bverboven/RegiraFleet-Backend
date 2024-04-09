using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Entities.Vehicles.VehicleTypes;

public class VehicleTypeRepository(FleetContextBase dbContext, IFleetAppContext appContext) : FleetRepositoryBase<VehicleType, VehicleTypeSearchObject>(dbContext, appContext)
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
            query = dbContext.FilterILikeTitle(query, qHelper.Parse(so.Title));
            // Q
            query = dbContext.FilterILikeTitleQ(query, qHelper.Parse(so.Q));
        }
        return query;
    }
}