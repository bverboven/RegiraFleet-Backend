using Microsoft.EntityFrameworkCore;
using Regira.Entities.Keywords;
using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Data.Extensions;

public static class QueryExtensions
{
    extension<T>(IQueryable<T> query) 
        where T : class, IHasNormalizedTitle, IHasCode
    {
        public IQueryable<T> FilterLikeTitleOrCode(IEnumerable<QKeyword>? keywords)
        {
            foreach (var kw in keywords ?? [])
            {
                query = query.Where(x => EF.Functions.Like(x.Code!, kw.TrimmedStartsWith!) 
                                         || EF.Functions.Like(x.NormalizedTitle!, $"{kw.Q}"));
            }

            return query;
        }

        public IQueryable<T> FilterLikeTitleOrCodeQ(IEnumerable<QKeyword>? keywords)
        {
            foreach (var kw in keywords ?? [])
            {
                query = query.Where(x => EF.Functions.Like(x.Code!, kw.TrimmedQW!) 
                                         || EF.Functions.Like(x.NormalizedTitle!, kw.Q!));
            }

            return query;
        }
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