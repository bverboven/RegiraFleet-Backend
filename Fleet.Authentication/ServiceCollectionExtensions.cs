using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Regira.Security.Authentication.Jwt.Extensions;
using Regira.Security.Authentication.Jwt.Models;
using Regira.Security.Authentication.Jwt.Services;

namespace Regira.Fleet.Authentication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFleetAuthentication(this IServiceCollection services, IConfiguration config)
    {
        // authentication
        var secret = "ACA_FLEET_SECRET:F061E1B7-363F-40F3-B052-0EE536792551:5A296568-C287-412F-836B-8E29ACD93A6A";
        services
            .Configure<IdentityOptions>(config.GetSection("Identity"))
            .AddScoped(p => p.GetRequiredService<IOptionsSnapshot<IdentityOptions>>().Value)
            .AddTransient(_ => new JwtTokenHelper(new JwtTokenOptions { Secret = secret }))
            .AddTransient(p =>
            {
                var users = p.GetRequiredService<IdentityOptions>().Users;
                return new UserManager(users);
            })
            .AddJwtAuthentication(options => options.Secret = secret);

        // authorization
        services
            .AddAuthorization(c =>
            {
                c.AddPolicy(FleetConstants.CanReadPolicy, b =>
                {
                    b.RequireAuthenticatedUser();
                    b.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    b.RequireClaim(FleetConstants.PermissionClaimType, FleetConstants.CanReadPermission);
                });
                c.AddPolicy(FleetConstants.CanEditPolicy, b =>
                {
                    b.RequireAuthenticatedUser();
                    b.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    b.RequireClaim(FleetConstants.PermissionClaimType, FleetConstants.CanEditPermission);
                });
            });

        return services;
    }
}