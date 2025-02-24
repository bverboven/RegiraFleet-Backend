using Regira.Entities.Abstractions;
using Regira.Entities.EFcore.Services;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Abstractions;

public abstract class FleetRepositoryBase<TEntity, TSearchObject>
    (FleetContextBase dbContext, IEntityReadService<TEntity, int, TSearchObject, EntitySortBy, EntityIncludes> readService, IEntityWriteService<TEntity, int> writeService) 
    : FleetRepositoryBase<TEntity, TSearchObject, EntitySortBy, EntityIncludes>(dbContext, readService, writeService) where TEntity : class, IEntity<int>, new()
    where TSearchObject : class, ISearchObject<int>, new();
public abstract class FleetRepositoryBase<TEntity, TSearchObject, TSortBy, TInclude>
    (FleetContextBase dbContext, IEntityReadService<TEntity, int, TSearchObject, TSortBy, TInclude> readService, IEntityWriteService<TEntity, int> writeService)
    : EntityRepository<TEntity, TSearchObject, TSortBy, TInclude>(readService, writeService)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : class, ISearchObject<int>, new()
    where TSortBy : struct, Enum
    where TInclude : struct, Enum
{
    protected readonly FleetContextBase DbContext = dbContext;
}