using Regira.DAL.EFcore.Normalizing;
using Regira.Entities.EFcore.Abstractions;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Abstractions;

public abstract class FleetRepositoryBase<TEntity, TSearchObject>(FleetContext dbContext)
    : FleetRepositoryBase<TEntity, TSearchObject, EntitySortBy, EntityIncludes>(dbContext)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : class, ISearchObject<int>, new();
public abstract class FleetRepositoryBase<TEntity, TSearchObject, TSortBy, TInclude>(FleetContext dbContext)
    : EntityRepositoryBase<FleetContext, TEntity, TSearchObject, TSortBy, TInclude>(dbContext)
    where TEntity : class, IEntity<int>, new()
    where TSearchObject : class, ISearchObject<int>, new()
    where TSortBy : struct, Enum
    where TInclude : struct, Enum
{
    public override async Task<int> SaveChanges(CancellationToken token = new())
    {
        DbContext.ApplyNormalizers();
        await DbContext.ApplyPrimers();

        return await base.SaveChanges(token);
    }
}