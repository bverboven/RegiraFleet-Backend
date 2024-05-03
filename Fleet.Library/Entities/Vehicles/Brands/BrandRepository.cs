using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.Vehicles.Brands;

namespace Regira.Fleet.Entities.Vehicles.Brands;

public class BrandRepository(FleetContextBase dbContext, IFleetAppContext appContext) : FleetRepositoryBase<Brand, BrandSearchObject>(dbContext, appContext)
{
    public override IQueryable<Brand> Filter(IQueryable<Brand> query, BrandSearchObject? so)
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