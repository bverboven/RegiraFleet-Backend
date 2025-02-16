using Microsoft.EntityFrameworkCore;
using Regira.Entities.Keywords;
using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Data.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> FilterLikeTitleOrCode<T>(this IQueryable<T> query, IEnumerable<QKeyword>? keywords)
        where T : class, IHasNormalizedTitle, IHasCode
    {
        foreach (var kw in keywords ?? [])
        {
            query = query.Where(x => EF.Functions.Like(x.Code!, $"{kw.Trimmed}%") 
                                     || EF.Functions.Like(x.NormalizedTitle!, $"{kw.Q}"));
        }

        return query;
    }
    public static IQueryable<T> FilterLikeTitleOrCodeQ<T>(this IQueryable<T> query, IEnumerable<QKeyword>? keywords)
        where T : class, IHasNormalizedTitle, IHasCode
    {
        foreach (var kw in keywords ?? [])
        {
            query = query.Where(x => EF.Functions.Like(x.Code!, kw.QW!) 
                                     || EF.Functions.Like(x.NormalizedTitle!, kw.Q!));
        }

        return query;
    }
    public static IQueryable<T> FilterLikeQ<T>(this IQueryable<T> query, IEnumerable<QKeyword>? keywords)
        where T : class, IHasNormalizedContent
    {
        foreach (var kw in keywords ?? [])
        {
            query = query.Where(x => EF.Functions.Like(x.NormalizedContent!, kw.QW!));
        }

        return query;
    }
}