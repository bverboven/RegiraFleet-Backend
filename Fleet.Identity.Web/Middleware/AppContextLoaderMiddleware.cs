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
            // only 1 tenantId claim should be present, unused tenantId claims are removed in middleware IdentityTenantUserClaimsService
            //var tenantId = context.User.Claims.SingleOrDefault(c => c.Type == FleetClaimTypes.TenantId)?.Value;
            var culture = context.User.Claims.FirstOrDefault(c => c.Type == FleetClaimTypes.Culture)?.Value;

            //if (!string.IsNullOrWhiteSpace(tenantId))
            //{
            //    await appContext.Tenant.Load(tenantId);
            //}
            appContext.Culture.Load(culture);
        }

        await next(context);
    }
}
