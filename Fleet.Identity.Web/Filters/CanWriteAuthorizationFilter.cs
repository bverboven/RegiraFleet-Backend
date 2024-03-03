using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Identity.Web.Filters;
public class CanWriteAuthorizationFilter : IAuthorizationFilter
{
    private string[] WriteActions => ["Create", "Modify", "Save"];

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            var routeData = context.RouteData;
            var action = context.RouteData.Values["action"]?.ToString();
            if (WriteActions.Any(a => a.Equals(action, StringComparison.InvariantCultureIgnoreCase)))
            {
                var hasWriteClaim = context.HttpContext.User.HasClaim(c => c.Type == ClientClaimTypes.Permission && c.Value == ClientPermissions.CanWrite);
                if (!hasWriteClaim)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
