using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Entities.Vehicles.VehicleTypes;

public class VehicleQueryFilter(IQKeywordHelper qHelper)
    : FilteredQueryBuilderBase<VehicleType, VehicleTypeSearchObject>
{
    public override IQueryable<VehicleType> Build(IQueryable<VehicleType> query, VehicleTypeSearchObject? so)
    {
        if (so != null)
        {
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