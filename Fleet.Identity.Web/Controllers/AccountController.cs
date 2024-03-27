using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Entities.Users;
using Regira.Fleet.Identity.Services;
using Regira.Fleet.Identity.Web.Models;
using Regira.Security.Authentication.Jwt.Extensions;
using Regira.Security.Authentication.Jwt.Services;
using Regira.Web.Utilities;
using System.Security.Claims;

namespace Regira.Fleet.Identity.Web.Controllers;

[ApiController]
[Route("auth")]
public class AccountController(JwtTokenHelper _tokenHelper, FleetUserIdentityManager _userManager, IUserClaimsPrincipalFactory<FleetUser> _claimsFactory, ILogger<AccountController> _logger) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("", Name = RouteNames.Authenticate)]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateInputDto model, [FromQuery] string clientApp, [FromQuery] string? clientId = null)
    {
        bool? isLockedOut = null;
        DateTimeOffset? lockedOutEnd = null;

        var user = await _userManager.FindByNameAsync(model.Username!);
        if (user != null)
        {
            isLockedOut = await _userManager.IsLockedOutAsync(user);
            if (isLockedOut == false)
            {
                bool isAuthenticated = await _userManager.CheckPasswordAsync(user, model.Password ?? string.Empty);
                if (isAuthenticated)
                {
                    var principal = await _claimsFactory.CreateAsync(user);
                    // check if user is linked to correct client
                    if (string.IsNullOrWhiteSpace(clientId) || principal.HasClaim(FleetClaimTypes.ClientId, clientId))
                    {
                        return Ok(CreateSuccessResponse(principal.Claims, clientApp));
                    }
                }
                // authentication failed
                await _userManager.AccessFailedAsync(user);
            }
            else
            {
                lockedOutEnd = await _userManager.GetLockoutEndDateAsync(user);
                _logger.LogWarning($"User {user.Id} {Request.GetIPAddress()} locked out until {lockedOutEnd:HH:mm:ss}");
            }
        }

        return StatusCode(StatusCodes.Status401Unauthorized, CreateFailedResponse(isLockedOut, lockedOutEnd));
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            // check if user is valid
            var exists = await _userManager.FindByIdAsync(User.FindUserId()!) != null;
            return exists ? NoContent() : Forbid();
        }

        return Unauthorized();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new
            {
                isAuthenticated = false
            });
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Unauthorized(new
            {
                isAuthenticated = false
            });
        }
        var principal = await _claimsFactory.CreateAsync(user);
        return Ok(CreateSuccessResponse(principal.Claims, User.FindFirstValue("aud")!));
    }


    [HttpGet("personal-data")]
    public async Task<IActionResult> GetPersonalData()
    {
        var user = await _userManager.FindByIdAsync(User.FindUserId()!);
        if (user == null)
        {
            return Unauthorized();
        }
        var principal = await _claimsFactory.CreateAsync(user);
        var personalDataClaimTypes = new[]
        {
            JwtClaimTypes.GivenName, JwtClaimTypes.FamilyName
        };
        var personalData = principal.Claims.Where(c => personalDataClaimTypes.Contains(c.Type))
            .ToDictionary(x => x.Type, x => x.Value);
        return Ok(personalData);
    }
    [HttpGet("permissions")]
    public async Task<IActionResult> GetPermissions()
    {
        var user = await _userManager.FindByIdAsync(User.FindUserId()!);
        if (user == null)
        {
            return Unauthorized();
        }
        var principal = await _claimsFactory.CreateAsync(user);
        var permissions = principal.Claims.Where(c => c.Type == ClientClaimTypes.Permission).Select(c => c.Value);
        return Ok(permissions);
    }


    protected AuthenticateResponseDto CreateFailedResponse(bool? isLockedOut = null, DateTimeOffset? lockedOutEnd = null)
    {
        return new AuthenticateResponseDto
        {
            IsLockedOut = isLockedOut,
            // datetime without timezone
            LockedOutEnd = lockedOutEnd.HasValue ? new DateTime(lockedOutEnd.Value.Ticks) : null
        };
    }
    protected AuthenticateResponseDto CreateSuccessResponse(IEnumerable<Claim> claims, string? audience = null)
    {
        var token = _tokenHelper.Create(claims, audience);
        return new AuthenticateResponseDto
        {
            IsAuthenticated = true,
            Token = token
        };
    }
}