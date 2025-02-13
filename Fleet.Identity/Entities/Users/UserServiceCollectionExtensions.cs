using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Identity.Models.Users;

namespace Regira.Fleet.Identity.Entities.Users;

public static class UserServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddFleetUsers<TContext>(this IEntityServiceCollection<TContext> services)
        where TContext : DbContext
    {
        services
            .For<FleetUserModel, string, FleetUserSearchObject, EntitySortBy, FleetUserIncludes>(e =>
            {
                e.UseEntityService<FleetUserRepository>();
                e.HasRepository<FleetUserRepository>();
            });
        return services;
    }
}