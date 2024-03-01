using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Regira.Security.Authentication.Jwt.Extensions;
using Regira.Security.Encryption;
using static Regira.Fleet.Identity.DependencyInjection.ServiceCollectionExtensions;

namespace Regira.CRM.Identity.Web.DependencyInjection;
public static class ServiceCollectionExtensions
{
    const string AUTH_SECRET = "ACA_FLEET_SECRET:F061E1B7-363F-40F3-B052-0EE536792551:5A296568-C287-412F-836B-8E29ACD93A6A";

    public static AuthenticationBuilder AddFleetIdentity(this IServiceCollection services, Action<FleetIdentityOptions> configure)
    {
        var options = new FleetIdentityOptions();
        configure.Invoke(options);

        services
            // authentication
            .AddFleetAuthentication(options);

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