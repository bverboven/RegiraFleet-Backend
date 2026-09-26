using Microsoft.EntityFrameworkCore;
using Regira.Entities.Keywords;
using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Data.PostgreSQL.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> FilterILikeTitleOrCode<T>(this IQueryable<T> query, IEnumerable<QKeyword>? keywords)
        where T : class, IHasNormalizedTitle, IHasCode
    {
        foreach (var kw in keywords ?? [])
        {
            query = query.Where(x => EF.Functions.ILike(x.Code!, kw.TrimmedStartsWith!) 
                                     || EF.Functions.ILike(x.NormalizedTitle!, $"{kw.Q}"));
        }

        return query;
    }
    public static IQueryable<T> FilterILikeTitleOrCodeQ<T>(this IQueryable<T> query, IEnumerable<QKeyword>? keywords)
        where T : class, IHasNormalizedTitle, IHasCode
    {
        foreach (var kw in keywords ?? [])
        {
            query = query.Where(x => EF.Functions.ILike(x.Code!, kw.TrimmedQW!) 
                                     || EF.Functions.ILike(x.NormalizedTitle!, kw.Q!));
        }

        return query;
    }
    public static IQueryable<T> FilterILikeQ<T>(this IQueryable<T> query, IEnumerable<QKeyword>? keywords)
        where T : class, IHasNormalizedContent
    {
        foreach (var kw in keywords ?? [])
        {
            query = query.Where(x => EF.Functions.ILike(x.NormalizedContent!, kw.QW!));
        }

        return query;
    }
}