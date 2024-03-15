using Microsoft.AspNetCore.Mvc;
using Regira.Fleet.Admin.Api.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.Clients;
using Regira.Fleet.Identity.Models;
using Regira.Fleet.Identity.Services;
using Regira.Fleet.Identity.Web.Extensions;
using System.Security.Claims;

namespace Fleet.Admin.Api.Controllers;

[ApiController]
[Route("admin/clients")]
public class ClientAdminController(FleetContext dbContext, ClientContext clientContext, FleetUserManager userManager) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] UserInputDto model)
    {
        var user = await userManager.FindByNameAsync(model.Email);
        if (user == null)
        {
            user = new FleetUser
            {
                Email = model.Email,
                UserName = model.Email
            };
        }

        var result = string.IsNullOrEmpty(model.Password)
            ? await userManager.CreateAsync(user)
            : await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return Ok();
        }


        ModelState.AddIdentityErrors(result.Errors);

        return BadRequest(ModelState);
    }

    [HttpPost("link")]
    public async Task<IActionResult> AddUserToClient([FromBody] string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return BadRequest();
        }

        var clientId = clientContext.ClientId;
        var permissions = new Claim[]
        {
           new Claim( ClientPermissions.CanRead,clientId.ToString())
        };
        var result = await userManager.AddClaimsAsync(user, permissions);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest();
    }
}
