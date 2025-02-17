using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Fleet.Data.PostgreSQL.Extensions;
using Regira.Fleet.Models.Vehicles.Brands;

namespace Regira.Fleet.DependencyInjection.Postgres;

public class BrandPostgresQueryFilter(IQKeywordHelper qHelper) : FilteredQueryBuilderBase<Brand, BrandSearchObject>
{
    public override IQueryable<Brand> Build(IQueryable<Brand> query, BrandSearchObject? so)
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