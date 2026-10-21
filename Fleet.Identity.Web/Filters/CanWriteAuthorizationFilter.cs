using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Identity.Web.Filters;

/// <summary>
/// Fails closed: a write passes only through a controller listed in <see cref="CanWriteAuthorizationOptions"/>, for a caller
/// holding one of the permissions its entry names. A controller without an entry refuses every write and logs which one it is.
/// Reads are every GET plus the POST list/search overloads; everything else writes, deletes and patches included.
/// It is an authorization filter, so it runs before model binding: an unauthorized write gets its 403 whatever its body holds.
/// </summary>
public class CanWriteAuthorizationFilter(IOptions<CanWriteAuthorizationOptions> options, ILogger<CanWriteAuthorizationFilter> logger) : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var action = context.ActionDescriptor as ControllerActionDescriptor;
        var method = context.HttpContext.Request.Method;

        var isRead = HttpMethods.IsGet(method)
            || (HttpMethods.IsPost(method) && action?.ActionName is "List" or "Search");
        // a write is public only when its ACTION says so (sign-in, password recovery, confirm-email):
        // [AllowAnonymous] on a controller opens its reads, never its writes
        var isPublicWrite = action?.MethodInfo.GetCustomAttributes(inherit: true).OfType<IAllowAnonymous>().Any() == true;
        if (isRead || isPublicWrite)
        {
            return Task.CompletedTask;
        }

        if (action == null || !options.Value.WritePermissions.TryGetValue(action.ControllerTypeInfo.AsType(), out var permissions))
        {
            logger.LogWarning("{Filter} has no entry for {Controller}; its writes are refused.",
                nameof(CanWriteAuthorizationFilter), action?.ControllerTypeInfo.Name ?? context.ActionDescriptor.DisplayName);
            context.Result = new ForbidResult();
        }
        else if (context.HttpContext.User.Identity?.IsAuthenticated != true)
        {
            context.Result = new ChallengeResult();
        }
        else if (permissions.Length > 0
                 && !context.HttpContext.User.HasClaim(c => c.Type == TenantClaimTypes.Permission && permissions.Contains(c.Value)))
        {
            context.Result = new ForbidResult();
        }

        return Task.CompletedTask;
    }
}

public class CanWriteAuthorizationOptions
{
    /// <summary>
    /// Per controller type, the permissions (any one of them) a write through it needs. An empty list lets every signed-in user write.
    /// </summary>
    public Dictionary<Type, string[]> WritePermissions { get; } = [];

    public CanWriteAuthorizationOptions Allow<TController>(params string[] permissions)
        where TController : ControllerBase
    {
        WritePermissions[typeof(TController)] = permissions;
        return this;
    }
}
