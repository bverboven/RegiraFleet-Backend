using Microsoft.EntityFrameworkCore;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleLikeQueryFilter(IQKeywordHelper qHelper) : FilteredQueryBuilderBase<Vehicle, VehicleSearchObject>
{
    public override IQueryable<Vehicle> Build(IQueryable<Vehicle> query, VehicleSearchObject? so)
    {
        if (so != null)
        {
            // Brand
            if (!string.IsNullOrWhiteSpace(so.Brand))
            {
                var kw = qHelper.ParseKeyword(so.Brand);
                query = query.Where(x => EF.Functions.Like(x.Brand!.Code, kw.Q) ||
                                         EF.Functions.Like(x.Brand.NormalizedTitle, kw.Q));
            }
            // VehicleType
            if (!string.IsNullOrWhiteSpace(so.VehicleType))
            {
                var kw = qHelper.ParseKeyword(so.VehicleType);
                query = query.Where(x => EF.Functions.Like(x.VehicleType!.Code, kw.Q) ||
                                         EF.Functions.Like(x.VehicleType.NormalizedTitle, kw.Q));
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var kw = qHelper.ParseKeyword(so.Title);
                query = query.Where(x => EF.Functions.Like(x.NormalizedTitle, kw.Q));
            }
        }

        return query;
    }
}