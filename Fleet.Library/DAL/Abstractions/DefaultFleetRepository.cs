using Regira.Entities.EFcore.Abstractions;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.DAL.Abstractions;

public class DefaultFleetRepository<TEntity, TSearchObject>(FleetContext dbContext)
    : DefaultFleetRepository<TEntity, TSearchObject, EntitySortBy, EntityIncludes>(dbContext)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : class, ISearchObject<int>, new();
public class DefaultFleetRepository<TEntity, TSearchObject, TSortBy, TInclude>(FleetContext dbContext)
    : EntityRepositoryBase<FleetContext, TEntity, TSearchObject, TSortBy, TInclude>(dbContext)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : class, ISearchObject<int>, new()
    where TSortBy : struct, Enum
    where TInclude : struct, Enum
{
}