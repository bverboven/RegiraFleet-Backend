using Regira.Entities.Abstractions;
using Regira.Entities.EFcore.Services;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Identity.Abstractions;

public abstract class IdentityRepositoryBase<TEntity, TSearchObject>
    (IEntityReadService<TEntity, string, TSearchObject, EntitySortBy, EntityIncludes> readService, IEntityWriteService<TEntity, string> writeService) 
    : IdentityRepositoryBase<TEntity, string, TSearchObject, EntitySortBy, EntityIncludes>(readService, writeService) 
    where TEntity : class, IEntity<string>, new()
    where TSearchObject : class, ISearchObject<string>, new();

public abstract class IdentityRepositoryBase<TEntity, TKey, TSearchObject, TSortBy, TInclude>
    (IEntityReadService<TEntity, TKey, TSearchObject, TSortBy, TInclude> readService, IEntityWriteService<TEntity, TKey> writeService) 
    : EntityRepository<TEntity, TKey, TSearchObject, TSortBy, TInclude>(readService, writeService)
    where TEntity : class, IEntity<TKey>, new()
    where TSearchObject : class, ISearchObject<TKey>, new()
    where TSortBy : struct, Enum
    where TInclude : struct, Enum;