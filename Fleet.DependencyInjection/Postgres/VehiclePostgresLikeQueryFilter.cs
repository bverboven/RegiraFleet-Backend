using Microsoft.EntityFrameworkCore;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Data.PostgreSQL.Extensions;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.DependencyInjection.Postgres;

public class VehiclePostgresLikeQueryFilter(IQKeywordHelper qHelper) : FilteredQueryBuilderBase<Vehicle, VehicleSearchObject>
{
    public override IQueryable<Vehicle> Build(IQueryable<Vehicle> query, VehicleSearchObject? so)
    {
        if (so != null)
        {
            // Brand
            if (!string.IsNullOrWhiteSpace(so.Brand))
            {
                var kw = qHelper.ParseKeyword(so.Brand);
                query = query.Where(x => EF.Functions.ILike(x.Brand!.Code!, kw.Q!) ||
                                         EF.Functions.ILike(x.Brand.NormalizedTitle!, kw.Q!));
            }
            // VehicleType
            if (!string.IsNullOrWhiteSpace(so.VehicleType))
            {
                var kw = qHelper.ParseKeyword(so.VehicleType);
                query = query.Where(x => EF.Functions.ILike(x.VehicleType!.Code!, kw.Q!) ||
                                         EF.Functions.ILike(x.VehicleType.NormalizedTitle!, kw.Q!));
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var keywords = qHelper.Parse(so.Title);
                query = query.FilterILikeTitleOrCode(keywords);
            }
        }

        return query;
    }
}