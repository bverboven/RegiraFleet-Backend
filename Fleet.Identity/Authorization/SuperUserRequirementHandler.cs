using Microsoft.AspNetCore.Authorization;
using Regira.Fleet.Core.Constants;
using System.Security.Claims;

namespace Regira.Fleet.Identity.Authorization;

public class SuperUserRequirement : IAuthorizationRequirement;
public class SuperUserRequirementHandler : AuthorizationHandler<SuperUserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperUserRequirement requirement)
    {
        if (context.User.HasClaim(ClaimTypes.Role, FleetClaimTypes.SuperUser))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new AuthorizationFailureReason(this, "Not a SuperUser"));
        }

        return Task.CompletedTask;
    }
}
