using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Services;
using Regira.Security.Authentication.Jwt.Abstraction;
using Regira.Security.Authentication.Jwt.Extensions;
using Regira.Security.Authentication.Web.Controllers;

namespace Regira.Fleet.Identity.Web.Controllers;

// sign-in, validate, refresh and personal-data come from the base, with its lockout, sign-in options and token claims
public class AccountController(ITokenHelper tokenHelper, FleetUserIdentityManager userManager, IUserClaimsPrincipalFactory<FleetUser> claimsFactory, ILogger<AccountController> logger)
    : AccountControllerBase<FleetUser>(tokenHelper, userManager, claimsFactory, logger)
{
    [HttpGet("permissions")]
    public async Task<IActionResult> GetPermissions([FromServices] FleetUserIdentityManager userManager, [FromServices] IUserClaimsPrincipalFactory<FleetUser> claimsFactory)
    {
        var user = await userManager.FindByIdAsync(User.FindUserId()!);
        if (user == null)
        {
            return Unauthorized();
        }
        var principal = await claimsFactory.CreateAsync(user);
        var permissions = principal.Claims.Where(c => c.Type == TenantClaimTypes.Permission).Select(c => c.Value);
        return Ok(permissions);
    }
}
