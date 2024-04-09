using Regira.Entities.Extensions;
using Regira.Entities.Keywords;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Data;
using Regira.Utilities;

namespace Regira.Fleet.Extensions;

public static class EntityCollectionExtensions
{
    public static IQueryable<T> FilterILikeTitle<T>(this FleetContextBase dbContext, IQueryable<T> query, IEnumerable<QKeyword> keywords)
        where T : class, IHasNormalizedTitle, IHasCode
    {
        if (keywords?.Any() == true)
        {
            foreach (var kw in keywords)
            {
                query = query.Where(x => dbContext.ILike(x.Code!, $"{kw.Trimmed}%") || dbContext.ILike(x.NormalizedTitle!, $"{kw.Q}"));
            }
        }

        return query;
    }
    public static IQueryable<T> FilterILikeTitleQ<T>(this FleetContextBase dbContext, IQueryable<T> query, IEnumerable<QKeyword> keywords)
        where T : class, IHasNormalizedTitle, IHasCode
    {
        if (keywords?.Any() == true)
        {
            foreach (var kw in keywords)
            {
                query = query.Where(x => dbContext.ILike(x.Code!, kw.QW!) || dbContext.ILike(x.NormalizedTitle!, kw.Q!));
            }
        }

        return query;
    }
    public static IQueryable<T> FilterILikeQ<T>(this FleetContextBase dbContext, IQueryable<T> query, IEnumerable<QKeyword> keywords)
        where T : class, IHasNormalizedContent
    {
        if (keywords?.Any() == true)
        {
            foreach (var kw in keywords)
            {
                query = query.Where(x => dbContext.ILike(x.NormalizedContent!, kw.QW!));
            }
        }

        return query;
    }

    public static ICollection<T> Prepare<T>(this ICollection<T> items)
    {
        var entityType = typeof(T);
        if (TypeUtility.ImplementsInterface<IEntity<int>>(entityType))
        {
            items.Cast<IEntity<int>>().AdjustIdForEfCore();
        }

        if (TypeUtility.ImplementsInterface<ISortable>(entityType))
        {
            items.Cast<ISortable>().SetSortOrder();
        }

        return items;
    }
}