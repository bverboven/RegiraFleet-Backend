using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.EFcore.Services;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Abstractions;

public abstract class FleetRepositoryBase<TEntity, TSearchObject>(FleetContextBase dbContext,
    IQueryBuilder<TEntity, TSearchObject, EntitySortBy, EntityIncludes> queryBuilder, IFleetAppContext appContext)
    : FleetRepositoryBase<TEntity, TSearchObject, EntitySortBy, EntityIncludes>(dbContext, queryBuilder, appContext)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : class, ISearchObject, new();
public abstract class FleetRepositoryBase<TEntity, TSearchObject, TSortBy, TInclude>(FleetContextBase dbContext,
    IQueryBuilder<TEntity, TSearchObject, TSortBy, TInclude> queryBuilder, IFleetAppContext appContext)
    : EntityRepository<FleetContextBase, TEntity, TSearchObject, TSortBy, TInclude>(dbContext, queryBuilder)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : class, ISearchObject, new()
    where TSortBy : struct, Enum
    where TInclude : struct, Enum
{
    protected readonly FleetContextBase DbContext = dbContext;
}