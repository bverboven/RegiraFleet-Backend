using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Identity.Entities.Clients;
using Regira.Fleet.Identity.Models.Clients;
using Regira.Fleet.Identity.Models.Clients.Subscriptions;

namespace Regira.Fleet.Identity.DependencyInjection.Entities;

public static class ClientServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddClients<TContext>(this IEntityServiceCollection<TContext> services)
        where TContext : DbContext
    {
        services
            .For<Client, string, ClientSearchObject, EntitySortBy, ClientIncludes>(e =>
            {
                e.UseEntityService<ClientRepository>();
                e.HasRepository<ClientRepository>();
            })
            .For<ClientSubscription, int, ClientSubscriptionSearchObject>();
        return services;
    }
}