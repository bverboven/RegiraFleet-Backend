using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Data;
using System.Security.Claims;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
public class ClientController(IAccountsDbContext dbContext) : ControllerBase
{
    [HttpGet("clients")]
    public async Task<IActionResult> List()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var items = await dbContext.Clients.Where(c => c.UserClaims!.Any(cc => cc.UserId == userId)).ToListAsync();
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
}
