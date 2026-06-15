using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.QueryBuilders;
using Regira.Entities.DependencyInjection.ServiceCollections.Abstractions;
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
                e.HasRepository<FleetUserRepository>();
                e.AddFilter<FleetUser, string, FleetUserSearchObject, UserQueryFilter>();
                if (dbType == DataBaseTypes.PostgreSQL)
                {
                    e.AddFilter<FleetUser, string, FleetUserSearchObject, UserPostgresLikeQueryFilter>();
                }
                else
                {
                    e.AddFilter<FleetUser, string, FleetUserSearchObject, UserLikeQueryFilter>();
                }
                
                e.UseMapping<FleetUserDto, FleetUserInputDto>()
                    .AfterInput((dto, model) =>
                    {
                        if (string.IsNullOrWhiteSpace(model.UserName))
                        {
                            model.UserName = dto.Email;
                        }
                        if (dto.UserClaims != null)
                        {
                            var userClaims = dto.UserClaims
                                .Select(x => new IdentityUserClaim<string> { Id = x.Id, ClaimType = x.ClaimType, ClaimValue = x.ClaimValue })
                                .ToList();
                            //if (!string.IsNullOrWhiteSpace(dto.GivenName))
                            //{
                            //    var claim = userClaims.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.GivenName);
                            //    if (claim != null)
                            //    {
                            //        claim.ClaimValue = dto.GivenName;
                            //    }
                            //    else
                            //    {
                            //        userClaims.Add(new IdentityUserClaim<string> { ClaimType = FleetClaimTypes.GivenName, ClaimValue = dto.GivenName });
                            //    }
                            //}
                            //if (!string.IsNullOrWhiteSpace(dto.LastName))
                            //{
                            //    var claim = userClaims.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.LastName);
                            //    if (claim != null)
                            //    {
                            //        claim.ClaimValue = dto.LastName;
                            //    }
                            //    else
                            //    {
                            //        userClaims.Add(new IdentityUserClaim<string> { ClaimType = FleetClaimTypes.LastName, ClaimValue = dto.LastName });
                            //    }
                            //}
                            model.UserClaims = userClaims;
                        }
                    });
            });
        return services;
    }
}