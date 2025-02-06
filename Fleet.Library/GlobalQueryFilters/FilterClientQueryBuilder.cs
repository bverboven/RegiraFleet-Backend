using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.GlobalQueryFilters
{
    public class FilterClientQueryBuilder(IFleetAppContext appContext) : FilterClientQueryBuilder<int>(appContext);
    public class FilterClientQueryBuilder<TKey>(IFleetAppContext appContext) : GlobalFilteredQueryBuilderBase<IHasClientId, TKey>
    {
        public override IQueryable<IHasClientId> Build(IQueryable<IHasClientId> query, ISearchObject<TKey>? _)
            => query.Where(x => x.ClientId == appContext.Client.ClientId);
    }
}
