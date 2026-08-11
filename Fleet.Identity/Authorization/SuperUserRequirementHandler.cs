using Microsoft.AspNetCore.Authorization;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Identity.Authorization;

public class SuperUserRequirement : IAuthorizationRequirement;
public class SuperUserRequirementHandler : AuthorizationHandler<SuperUserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperUserRequirement requirement)
    {
        // IsInRole resolves through each scheme's own RoleClaimType — "role" for JWT bearer, the long
        // ClaimTypes.Role URI for API keys — so it holds for both. Pinning a single spelling does not.
        if (context.User.IsInRole(FleetClaimTypes.SuperUser))
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
