using Regira.DAL.EFcore.Normalizing;
using Regira.Entities.EFcore.Abstractions;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Identity.Data;
using Regira.Utilities;

namespace Regira.Fleet.Identity.Abstractions;

public abstract class IdentityRepositoryBase<TEntity, TSearchObject>(AccountsContext dbContext)
    : IdentityRepositoryBase<TEntity, string, TSearchObject, EntitySortBy, EntityIncludes>(dbContext)
    where TEntity : class, IEntity<string>, new()
    where TSearchObject : class, ISearchObject<string>, new();
public abstract class IdentityRepositoryBase<TEntity, TKey, TSearchObject, TSortBy, TInclude>(AccountsContext dbContext)
    : EntityRepositoryBase<AccountsContext, TEntity, TKey, TSearchObject, TSortBy, TInclude>(dbContext)
    where TEntity : class, IEntity<TKey>, new()
    where TSearchObject : class, ISearchObject<TKey>, new()
    where TSortBy : struct, Enum
    where TInclude : struct, Enum
{
    public override IQueryable<TEntity> Filter(IQueryable<TEntity> query, TSearchObject? so)
    {
        if (so != null)
        {
            query = query.FilterId(so.Id);
            query = query.FilterIds(so.Ids);
            query = query.FilterExclude(so.Exclude);

            if (TypeUtility.ImplementsInterface<IHasCreated>(typeof(TEntity)))
            {
                query = query.Cast<IHasCreated>().FilterCreated(so.MinCreated, so.MaxCreated).Cast<TEntity>();
            }
            if (TypeUtility.ImplementsInterface<IHasLastModified>(typeof(TEntity)))
            {
                query = query.Cast<IHasLastModified>().FilterLastModified(so.MinLastModified, so.MaxLastModified).Cast<TEntity>();
            }
            if (TypeUtility.ImplementsInterface<IArchivable>(typeof(TEntity)))
            {
                query = query.Cast<IArchivable>().FilterArchivable(so.IsArchived).Cast<TEntity>();
            }
        }

        return query;
    }


    public override async Task<int> SaveChanges(CancellationToken token = new())
    {
        DbContext.ApplyNormalizers();
        await DbContext.ApplyPrimers();

        return await base.SaveChanges(token);
    }
}