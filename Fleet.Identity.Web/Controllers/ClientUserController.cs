using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Regira.Fleet.Identity.Models;
using Regira.Fleet.Identity.Services;
using Regira.Fleet.Identity.Web.Extensions;
using Regira.Fleet.Identity.Web.Models;
using System.Security.Claims;

namespace Regira.Fleet.Identity.Web.Controllers;

[Authorize("IsAdmin")]
[ApiController]
[Route("accounts")]
public class ClientUserController(FleetUserManager userManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserInputDto input)
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
}