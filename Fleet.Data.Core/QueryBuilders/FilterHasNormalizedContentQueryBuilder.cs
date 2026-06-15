using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Data.Extensions;

namespace Regira.Fleet.Data.QueryBuilders;

public class FilterHasNormalizedContentQueryBuilder(IQKeywordHelper qHelper) : FilterHasNormalizedContentQueryBuilder<int>(qHelper);
public class FilterHasNormalizedContentQueryBuilder<TKey>(IQKeywordHelper qHelper) : GlobalFilteredQueryBuilderBase<IHasNormalizedContent, TKey>
{
    public override IQueryable<IHasNormalizedContent> Build(IQueryable<IHasNormalizedContent> query, ISearchObject<TKey>? so)
    {
        if (!string.IsNullOrWhiteSpace(so?.Q))
        {
            var keywords = qHelper.Parse(so.Q);
            query = query.FilterLikeQ(keywords);
        }

        return query;
    }
}