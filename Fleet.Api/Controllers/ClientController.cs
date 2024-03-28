using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Data;
using System.Security.Claims;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("clients")]
public class ClientController(AccountsContext dbContext) : ControllerBase
{
    [HttpGet]
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
}

[ApiController]
[Route("demo-clients")]
public class DemoClientController(AccountsContext dbContext) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> List()
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
