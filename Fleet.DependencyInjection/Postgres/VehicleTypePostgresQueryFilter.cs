using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Data.PostgreSQL.Extensions;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.DependencyInjection.Postgres;

public class VehicleTypePostgresQueryFilter(IQKeywordHelper qHelper)
    : FilteredQueryBuilderBase<VehicleType, VehicleTypeSearchObject>
{
    public override IQueryable<VehicleType> Build(IQueryable<VehicleType> query, VehicleTypeSearchObject? so)
    {
        if (so != null)
        {
            // Code
            query = query.FilterCode(so.Code);
            // Title
            query = query.FilterILikeTitleOrCode(qHelper.Parse(so.Title));
            // Q
            query = query.FilterILikeTitleOrCodeQ(qHelper.Parse(so.Q));
        }
        return query;
    }
}