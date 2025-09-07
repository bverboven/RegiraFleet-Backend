using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Identity.Web.Filters;

public class CanReadAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isAuthenticated = context.HttpContext.User.Identity?.IsAuthenticated == true;
        var isAuthenticating = RouteNames.Authenticate.Equals(context.RouteData.Values["action"]?.ToString(), StringComparison.InvariantCultureIgnoreCase);
        // user may still be authenticated with earlier (now invalid) token
        // make sure he can start a new one when requested
        if (isAuthenticated && !isAuthenticating)
        {
            var hasReadClaim = context.HttpContext.User.HasClaim(c => c is { Type: TenantClaimTypes.Permission, Value: TenantPermissions.CanRead });
            if (!hasReadClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
