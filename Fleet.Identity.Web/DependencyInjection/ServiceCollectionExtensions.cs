using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Authorization;
using Regira.Fleet.Identity.DependencyInjection;
using Regira.Security.Authentication.Jwt.Extensions;
using Regira.Security.Encryption;

namespace Regira.Fleet.Identity.Web.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static AuthenticationBuilder AddFleetIdentity(this IServiceCollection services, Action<FleetIdentityOptions> configure)
    {
        var options = new FleetIdentityOptions();
        configure.Invoke(options);

        // Authentication
        services
            .AddFleetAuthentication(options);

        // Authorization
        services
            .AddAuthorization(auth =>
            {
                auth.AddPolicy(FleetPolicies.CanReadPolicy, o => o.RequireClaim(FleetClaimTypes.Permission, TenantPermissions.CanRead));
                auth.AddPolicy(FleetPolicies.CanWritePolicy, o => o.RequireClaim(FleetClaimTypes.Permission, TenantPermissions.CanWrite));
                auth.AddPolicy(FleetPolicies.AdminPolicy, o => o.RequireClaim(FleetClaimTypes.Permission, TenantPermissions.Administrator));
                auth.AddPolicy(FleetPolicies.SuperUserPolicy, o =>
                {
                    o.Requirements.Add(new SuperUserRequirement());
                });
            });

        return services
            // jwt
            .AddJwtAuthentication(c =>
            {
                var encrypter = new SymmetricEncrypter();
                var secretKey = options.SecretKey;
                if (string.IsNullOrWhiteSpace(secretKey))
                {
                    throw new NullReferenceException($"No SecretKey found in {nameof(IConfiguration)}");
                }
                var jwtSecret = encrypter.Decrypt(secretKey);

                c.Secret = jwtSecret;
                c.Authority = "identity-api";
                c.Audiences = options.Audiences;
            });
    }
}