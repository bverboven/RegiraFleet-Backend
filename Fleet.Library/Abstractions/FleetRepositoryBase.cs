using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Abstractions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
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
    : EntityRepositoryBase<FleetContextBase, TEntity, TSearchObject, TSortBy, TInclude>(dbContext, queryBuilder)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : class, ISearchObject, new()
    where TSortBy : struct, Enum
    where TInclude : struct, Enum
{
    private readonly FleetContextBase _dbContext = dbContext;

    public override Task Remove(TEntity item)
    {
        if (item is IArchivable archivableItem)
        {
            archivableItem.IsArchived = true;
            _dbContext.Entry(item).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        return base.Remove(item);
    }

    public override void PrepareItem(TEntity item)
    {
        base.PrepareItem(item);
        if (item is IHasClientId itemWithClientId && string.IsNullOrWhiteSpace(itemWithClientId.ClientId))
        {
            itemWithClientId.ClientId = appContext.Client.ClientId!;
        }
    }
}