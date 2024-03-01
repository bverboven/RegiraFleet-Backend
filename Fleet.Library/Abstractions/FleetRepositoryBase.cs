using Regira.DAL.EFcore.Normalizing;
using Regira.Entities.EFcore.Abstractions;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Utilities;

namespace Regira.Fleet.Abstractions;

public abstract class FleetRepositoryBase<TEntity, TSearchObject>(FleetContext dbContext, IFleetAppContext appContext)
    : FleetRepositoryBase<TEntity, TSearchObject, EntitySortBy, EntityIncludes>(dbContext, appContext)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : FleetSearchObject, new();
public abstract class FleetRepositoryBase<TEntity, TSearchObject, TSortBy, TInclude>(FleetContext dbContext, IFleetAppContext appContext)
    : EntityRepositoryBase<FleetContext, TEntity, TSearchObject, TSortBy, TInclude>(dbContext)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : FleetSearchObject, new()
    where TSortBy : struct, Enum
    where TInclude : struct, Enum
{
    public override async Task<TEntity?> Details(int id)
    {
        var item = await base.Details(id);
        if (item is IHasClientId hasClientId && hasClientId.ClientId != appContext.Client.ClientId)
        {
            // make sure only allowed clientId items are accessed
            return null;
        }

        return item;
    }
    public override IQueryable<TEntity> Filter(IQueryable<TEntity> query, TSearchObject? so)
    {
        query = base.Filter(query, so);

        // make sure only allowed clientId items are loaded
        if (TypeUtility.ImplementsInterface<IHasClientId>(typeof(TEntity)))
        {
            query = query.Where(x => (x as IHasClientId)!.ClientId == appContext.Client.ClientId);
        }

        return query;
    }


    public override async Task<int> SaveChanges(CancellationToken token = new())
    {
        DbContext.ApplyNormalizers();
        await DbContext.ApplyPrimers();

        return await base.SaveChanges(token);
    }

    public override void PrepareItem(TEntity item)
    {
        base.PrepareItem(item);
        if (item is IHasClientId itemWithClientId && itemWithClientId.ClientId <= 0)
        {
            itemWithClientId.ClientId = appContext.Client.ClientId;
        }
    }
}