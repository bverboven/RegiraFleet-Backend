using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Identity.Web.Filters;

public class CanReadAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            var hasReadClaim = context.HttpContext.User.HasClaim(c => c.Type == ClientClaimTypes.Permission && c.Value == ClientPermissions.CanRead);
            if (!hasReadClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
