using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Vehicles.Brands;

namespace Regira.Fleet.Entities.Vehicles.Brands;

public class BrandRepository(
    FleetContextBase dbContext, IFleetAppContext appContext,
    IQueryBuilder<Brand, BrandSearchObject, EntitySortBy, EntityIncludes> queryBuilder)
    : FleetRepositoryBase<Brand, BrandSearchObject, EntitySortBy, EntityIncludes>(dbContext, queryBuilder, appContext);