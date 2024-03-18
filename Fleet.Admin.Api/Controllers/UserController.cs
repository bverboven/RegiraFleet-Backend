using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Regira.Fleet.Admin.Api.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Data;
using Regira.Fleet.Identity.Models;
using Regira.Fleet.Identity.Services;
using Regira.Fleet.Identity.Web.Extensions;
using Regira.Fleet.Identity.Web.Models;
using System.Security.Claims;

namespace Regira.Fleet.Admin.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(FleetContext dbContext, FleetUserManager userManager) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] UserInputDto input)
    {
        var user = new FleetUser
        {
            Email = input.Username,
            UserName = input.Username,
            Culture = input.Culture
        };

        var response = await userManager.CreateAsync(user, input.Password);

        if (response.Succeeded)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrWhiteSpace(input.GivenName))
            {
                claims.Add(new Claim(ClaimTypes.GivenName, input.GivenName));
            }
            if (!string.IsNullOrWhiteSpace(input.Surname))
            {
                claims.Add(new Claim(ClaimTypes.Surname, input.Surname));
            }

            return Ok(new
            {
                success = true
            });
        }

        if (response.Errors.Any())
        {
            ModelState.AddIdentityErrors(response.Errors);

            return BadRequest(ModelState);
        }

        return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error");
    }

    [HttpPost("link")]
    public async Task<IActionResult> AddUserToClient([FromBody] UserToClientInputDto input)
    {
        var user = await userManager.FindByIdAsync(input.UserId);
        if (user == null)
        {
            return NotFound();
        }

        var client = await dbContext.Clients.FindAsync(input.ClientId);
        if (client == null)
        {
            return NotFound();
        }

        var clientClaim = new Claim(ClientPermissions.CanRead, client.Guid);
        var result = await userManager.AddClaimAsync(user, clientClaim);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest();
    }
}
