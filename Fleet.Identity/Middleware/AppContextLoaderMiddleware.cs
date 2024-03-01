using Microsoft.AspNetCore.Http;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Identity.Constants;

namespace Regira.Fleet.Identity.Middleware;

public class AppContextLoaderMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IFleetAppContext appContext)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var clientId = context.User.Claims.Single(c => c.Type == FleetClaimTypes.ClientId).Value;
            var culture = context.User.Claims.FirstOrDefault(c => c.Type == FleetClaimTypes.Culture)?.Value;

            await appContext.Client.Load(clientId);
            appContext.Culture.Load(culture);
        }

        await next(context);
    }
}
