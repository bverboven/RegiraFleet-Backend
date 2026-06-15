using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.ServiceCollections.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Identity.Entities.Tenants;
using Regira.Fleet.Identity.Models.Tenants;
using Regira.Fleet.Identity.Models.Tenants.Subscriptions;

namespace Regira.Fleet.Identity.DependencyInjection.Entities;

public static class TenantServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddTenants<TContext>(this IEntityServiceCollection<TContext> services)
        where TContext : DbContext
    {
        services
            .For<Tenant, string, TenantSearchObject, EntitySortBy, TenantIncludes>(e =>
            {
                e.AddFilter<TenantFilteredQueryBuilder>();
                e.AddIncludes<TenantIncludableQueryBuilder>();
                e.Related<TenantSubscription, int>(c => c.Subscriptions);
            })
            .For<TenantSubscription, int, TenantSubscriptionSearchObject>();
        return services;
    }
}