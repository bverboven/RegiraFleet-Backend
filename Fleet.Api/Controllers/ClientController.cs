using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using System.Security.Claims;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
public class ClientController(AccountsContext dbContext) : ControllerBase
{
    [HttpGet("clients")]
    public async Task<IActionResult> List()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var items = await dbContext.Clients.Where(c => c.UserClaims!.Any(c => c.UserId == userId)).ToListAsync();
        var models = items.Select(x => new
        {
            x.Id,
            x.Code,
            x.Title,
            x.DefaultCulture
        });
        return Ok(models);
    }
    [AllowAnonymous]
    [HttpGet("demo-clients")]
    public async Task<IActionResult> ListDemo()
    {
        var items = await dbContext.Clients.ToListAsync();
        var models = items.Select(x => new
        {
            x.Id,
            x.Code,
            x.Title,
            x.DefaultCulture
        });
        return Ok(models);
    }

    [Authorize(FleetPolicies.AdminPolicy)]
    [HttpGet("users")]
    public async Task<IActionResult> ListClientUsers()
    {
        var clientId = User.FindFirstValue(FleetClaimTypes.ClientId);
        var items = await dbContext.Users
            .Where(u => u.ClientClaims!.Any(x => x.ClientId == clientId))
            .ToListAsync();
        var models = items.Select(x => new
        {
            x.Id,
            x.Email,
            x.UserClaims
        });
        return Ok(models);
    }
}
