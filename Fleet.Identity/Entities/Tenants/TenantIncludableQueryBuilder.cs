using Microsoft.EntityFrameworkCore;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Identity.Models.Tenants;

namespace Regira.Fleet.Identity.Entities.Tenants;

public class TenantIncludableQueryBuilder : IIncludableQueryBuilder<Tenant, string, TenantIncludes>
{
    public IQueryable<Tenant> AddIncludes(IQueryable<Tenant> query, TenantIncludes? includes = null)
    {
        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(TenantIncludes.Languages))
            {
                query = query.Include(x => Enumerable.OrderBy<TenantLanguage, string>(x.Languages!, a => a.LangCode));
            }
            if (includes.Value.HasFlag(TenantIncludes.Subscriptions))
            {
                query = query.Include(x => x.Subscriptions!.OrderByDescending(s => s.EndDate ?? s.StartDate ?? s.Created));
            }
            else if (includes.Value.HasFlag(TenantIncludes.ActiveSubscription))
            {
                query = query.Include(x => x.Subscriptions!.Where(s => s.StartDate <= DateTime.Now && (s.EndDate == null || s.EndDate >= DateTime.Today)));
            }
        }

        return query;
    }
}