using Microsoft.AspNetCore.Http;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Identity.Web.Middleware;

public class AppContextLoaderMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IFleetAppContext appContext)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            // only 1 clientId claim should be present, unused clientId claims are removed in middleware IdentityClientUserClaimsService
            var clientId = context.User.Claims.SingleOrDefault(c => c.Type == FleetClaimTypes.ClientId)?.Value;
            var culture = context.User.Claims.FirstOrDefault(c => c.Type == FleetClaimTypes.Culture)?.Value;

            if (!string.IsNullOrWhiteSpace(clientId))
            {
                await appContext.Client.Load(clientId);
            }
            appContext.Culture.Load(culture);
        }

        await next(context);
    }
}
