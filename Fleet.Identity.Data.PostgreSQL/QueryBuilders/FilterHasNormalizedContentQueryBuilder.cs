using Microsoft.EntityFrameworkCore;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;

namespace Regira.Fleet.Identity.Data.PostgreSQL.QueryBuilders;

public class FilterHasNormalizedContentQueryBuilder(IQKeywordHelper qHelper) : FilterHasNormalizedContentQueryBuilder<int>(qHelper);
public class FilterHasNormalizedContentQueryBuilder<TKey>(IQKeywordHelper qHelper) : GlobalFilteredQueryBuilderBase<IHasNormalizedContent, TKey>
{
    public override IQueryable<IHasNormalizedContent> Build(IQueryable<IHasNormalizedContent> query, ISearchObject<TKey>? so)
    {
        if (!string.IsNullOrWhiteSpace(so?.Q))
        {
            var keywords = qHelper.Parse(so.Q);
            query = keywords.Aggregate(
                query, 
                (filteredQuery, q) => filteredQuery
                    .Where(x => EF.Functions.ILike(x.NormalizedContent!, q.QW!))
            );
        }

        return query;
    }
}