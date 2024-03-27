using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Authorization;
using Regira.Security.Authentication.Jwt.Extensions;
using Regira.Security.Encryption;
using static Regira.Fleet.Identity.DependencyInjection.ServiceCollectionExtensions;

namespace Regira.Fleet.Identity.Web.DependencyInjection;
public static class ServiceCollectionExtensions
{
    const string AUTH_SECRET = "ACA_FLEET_SECRET:F061E1B7-363F-40F3-B052-0EE536792551:5A296568-C287-412F-836B-8E29ACD93A6A";

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
                auth.AddPolicy(FleetPolicies.CanReadPolicy, o => o.RequireClaim(FleetClaimTypes.Permission, ClientPermissions.CanRead));
                auth.AddPolicy(FleetPolicies.AdminPolicy, o => o.RequireClaim(FleetClaimTypes.Permission, ClientPermissions.Administrator));
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
                var decryptedKey = encrypter.Decrypt(secretKey);
                var jwtSecret = $"{AUTH_SECRET}:{decryptedKey}";

                c.Secret = jwtSecret;
                c.Authority = "identity-api";
                c.Audiences = options.Audiences;
            });
    }
}