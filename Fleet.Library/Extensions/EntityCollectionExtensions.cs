using Microsoft.EntityFrameworkCore;
using Regira.Entities.Extensions;
using Regira.Entities.Keywords;
using Regira.Entities.Models.Abstractions;
using Regira.Utilities;

namespace Regira.Fleet.Extensions;

public static class EntityCollectionExtensions
{
    public static IQueryable<T> FilterILikeTitle<T>(this IQueryable<T> query, IEnumerable<QKeyword>? keywords)
        where T : class, IHasNormalizedTitle, IHasCode
    {
        if (keywords != null)
        {
            foreach (var kw in keywords)
            {
                var trimmedUpper = kw.Trimmed!.ToUpper();
                query = query.Where(x => EF.Functions.Like(x.Code!.ToUpper(), $"{trimmedUpper}%") || EF.Functions.Like(x.NormalizedTitle!, $"{kw.Q}"));
            }
        }

        return query;
    }
    public static IQueryable<T> FilterILikeTitleQ<T>(this IQueryable<T> query, IEnumerable<QKeyword>? keywords)
        where T : class, IHasNormalizedTitle, IHasCode
    {
        if (keywords != null)
        {
            foreach (var kw in keywords)
            {
                query = query.Where(x => EF.Functions.Like(x.Code!.ToUpper(), kw.QW!) || EF.Functions.Like(x.NormalizedTitle!.ToUpper(), kw.Q!));
            }
        }

        return query;
    }
    public static IQueryable<T> FilterILikeQ<T>(this IQueryable<T> query, IEnumerable<QKeyword>? keywords)
        where T : class, IHasNormalizedContent
    {
        if (keywords != null)
        {
            foreach (var kw in keywords)
            {
                query = query.Where(x => EF.Functions.Like(x.NormalizedContent!, kw.QW!));
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

        if (items is ICollection<ISortable> sortableItems)
        {
            sortableItems.SetSortOrder();
        }
        if (TypeUtility.ImplementsInterface<ISortable>(entityType))
        {
            items.Cast<ISortable>().SetSortOrder();
        }

        return items;
    }
}