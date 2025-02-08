using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.EFcore.Services;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Identity.Data;

namespace Regira.Fleet.Identity.Abstractions;

public abstract class IdentityRepositoryBase<TEntity, TSearchObject>(
    AccountsContextBase dbContext,
    IQueryBuilder<TEntity, string, TSearchObject, EntitySortBy, EntityIncludes> queryBuilder)
    : IdentityRepositoryBase<TEntity, string, TSearchObject, EntitySortBy, EntityIncludes>(dbContext, queryBuilder)
    where TEntity : class, IEntity<string>, new()
    where TSearchObject : class, ISearchObject<string>, new();
public abstract class IdentityRepositoryBase<TEntity, TKey, TSearchObject, TSortBy, TInclude>(
    AccountsContextBase dbContext,
    IQueryBuilder<TEntity, TKey, TSearchObject, TSortBy, TInclude> queryBuilder)
    : EntityRepository<AccountsContextBase, TEntity, TKey, TSearchObject, TSortBy, TInclude>(dbContext, queryBuilder)
    where TEntity : class, IEntity<TKey>, new()
    where TSearchObject : class, ISearchObject<TKey>, new()
    where TSortBy : struct, Enum
    where TInclude : struct, Enum;