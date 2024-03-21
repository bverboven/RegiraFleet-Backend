using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Admin.Api.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Entities.Users;
using Regira.Fleet.Identity.Services;
using System.Security.Claims;

namespace Regira.Fleet.Admin.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("users")]
public class UserController(AccountsContext dbContext, FleetUserIdentityManager userManager) : EntityControllerBase<FleetUser, string, FleetUserSearchObject, EntitySortBy, EntityIncludes, FleetUserDto, FleetUserDto>
{
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

        var clientClaim = new Claim(ClientPermissions.CanRead, client.Id);
        var result = await userManager.AddClaimAsync(user, clientClaim);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest();
    }
}
