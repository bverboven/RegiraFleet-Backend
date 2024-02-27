using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Regira.Fleet.Api.Models.Authentication;
using Regira.Fleet.Authentication;
using Regira.Security.Authentication.Jwt.Services;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("auth")]
public class AccountController(JwtTokenHelper tokenHelper, UserManager userManager) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Authenticate([FromBody] AuthenticateInputDto model)
    {
        var user = userManager.Verify(model.Username, model.Password);
        if (user == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new
            {
                isAuthenticated = false
            });
        }

        var claims = new List<Claim>
        {
            new (JwtClaimTypes.Id,model.Username),
        };
        if (!string.IsNullOrWhiteSpace(user.DisplayName))
        {
            claims.Add(new(JwtClaimTypes.Name, user.DisplayName));
        }
        claims.AddRange(user.Permissions.Select(p => new Claim("permission", p)));

        var token = tokenHelper.Create(claims: claims);


        return Ok(CreateResponse(claims, token));
    }

    [HttpPost("validate")]
    public IActionResult Validate()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];
        return Ok(CreateResponse(User.Claims.ToList(), token));
    }
    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
        var token = tokenHelper.Create(User.Claims);
        return Ok(CreateResponse(User.Claims.ToList(), token));
    }

    protected AuthenticateResponseDto CreateResponse(IList<Claim> claims, string token)
    {
        var permissions = claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
        var displayName = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value;

        return new AuthenticateResponseDto
        {
            IsAuthenticated = true,
            DisplayName = displayName,
            Permissions = permissions,
            Token = token
        };
    }
}