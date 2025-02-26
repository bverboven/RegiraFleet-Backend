using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.Vehicles.Brands;

namespace Regira.Fleet.Entities.Vehicles.Brands;

public class BrandQueryFilter(IQKeywordHelper qHelper) : FilteredQueryBuilderBase<Brand, BrandSearchObject>
{
    public override IQueryable<Brand> Build(IQueryable<Brand> query, BrandSearchObject? so)
    {
        if (so != null)
        {
            // Code
            query = query.FilterCode(so.Code);
            // Title
            query = query.FilterLikeTitleOrCode(qHelper.Parse(so.Title));
            // Q
            query = query.FilterLikeTitleOrCodeQ(qHelper.Parse(so.Q));
        }
        return query;
    }
}