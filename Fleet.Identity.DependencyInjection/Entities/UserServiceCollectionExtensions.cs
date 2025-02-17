using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.DependencyInjection.QueryBuilders;
using Regira.Entities.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.DependencyInjection.Postgres;
using Regira.Fleet.Identity.Entities.Users;
using Regira.Fleet.Identity.Models.Users;

namespace Regira.Fleet.Identity.DependencyInjection.Entities;

public static class UserServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddFleetUsers<TContext>(this IEntityServiceCollection<TContext> services, string dbType)
        where TContext : DbContext
    {
        services
            .For<FleetUserModel, string, FleetUserSearchObject, EntitySortBy, FleetUserIncludes>(e =>
            {
                e.UseEntityService<FleetUserRepository>();
                e.HasRepository<FleetUserRepository>();
                e.AddQueryFilter<FleetUser, string, FleetUserSearchObject, UserQueryFilter>();
                if (dbType == DataBaseTypes.PostgreSQL)
                {
                    e.AddQueryFilter<FleetUser, string, FleetUserSearchObject, UserPostgresLikeQueryFilter>();
                }
                else
                {
                    e.AddQueryFilter<FleetUser, string, FleetUserSearchObject, UserLikeQueryFilter>();
                }
            });
        return services;
    }
}