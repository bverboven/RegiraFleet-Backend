using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Core.GlobalQueryFilters;

public class FilterHasTenantQueryBuilder(IFleetAppContext appContext) : FilterHasTenantQueryBuilder<int>(appContext);
public class FilterHasTenantQueryBuilder<TKey>(IFleetAppContext appContext) : GlobalFilteredQueryBuilderBase<IHasTenantId, TKey>
{
    public override IQueryable<IHasTenantId> Build(IQueryable<IHasTenantId> query, ISearchObject<TKey>? _)
        => query.Where(x => x.TenantId == appContext.Tenant.TenantId);
}