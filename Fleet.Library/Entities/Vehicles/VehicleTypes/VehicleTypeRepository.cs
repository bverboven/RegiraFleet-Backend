using Microsoft.EntityFrameworkCore;
using Regira.DAL.Paging;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Entities.Vehicles.VehicleTypes;

public class VehicleTypeRepository(FleetContextBase dbContext, IFleetAppContext appContext,
    IQueryBuilder<VehicleType, VehicleTypeSearchObject, EntitySortBy, EntityIncludes> queryBuilder)
    : FleetRepositoryBase<VehicleType, VehicleTypeSearchObject>(dbContext, queryBuilder, appContext)
{
    private readonly FleetContextBase _dbContext1 = dbContext;

    public override IQueryable<VehicleType> Query(IQueryable<VehicleType> query, IList<VehicleTypeSearchObject?> searchObjects, IList<EntitySortBy> sortBy, EntityIncludes? includes, PagingInfo? pagingInfo)
    {
        query = query.Include(x => x.Translations);
        return base.Query(query, searchObjects, sortBy, includes, pagingInfo);
    }

    public override void Modify(VehicleType item, VehicleType original)
    {
        base.Modify(item, original);

        _dbContext1.UpdateEntityChildCollection(original, item, x => x.Translations, (x, collection) => x.Translations = collection);
    }
    public override void PrepareItem(VehicleType item)
    {
        base.PrepareItem(item);

        item.Translations?.Prepare();
    }
}