using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Core.GlobalQueryFilters;

public class FilterHasClientQueryBuilder(IFleetAppContext appContext) : FilterHasClientQueryBuilder<int>(appContext);
public class FilterHasClientQueryBuilder<TKey>(IFleetAppContext appContext) : GlobalFilteredQueryBuilderBase<IHasClientId, TKey>
{
    public override IQueryable<IHasClientId> Build(IQueryable<IHasClientId> query, ISearchObject<TKey>? _)
        => query.Where(x => x.ClientId == appContext.Client.ClientId);
}