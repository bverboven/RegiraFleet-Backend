using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Entities.Models;
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
            query = query.FilterILikeTitle(qHelper.Parse(so.Title));
            // Q
            query = query.FilterILikeTitleQ(qHelper.Parse(so.Q));
        }
        return query;
    }

    public override IQueryable<VehicleType> AddIncludes(IQueryable<VehicleType> query, EntityIncludes? includes)
    {
        return query.Include(x => x.Translations);
    }

    public override void Modify(VehicleType item, VehicleType original)
    {
        base.Modify(item, original);

        dbContext.UpdateEntityChildCollection(original, item, x => x.Translations, (x, collection) => x.Translations = collection);
    }
    public override void PrepareItem(VehicleType item)
    {
        base.PrepareItem(item);

        item.Translations?.Prepare();
    }
}