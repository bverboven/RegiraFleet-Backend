using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Data;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
public class TenantController(IAccountsDbContext dbContext) : ControllerBase
{
    [HttpGet("tenants")]
    public async Task<IActionResult> List()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var items = await dbContext.Tenants.Where(c => c.UserClaims!.Any(cc => cc.UserId == userId)).ToListAsync();
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
    [HttpGet("demo-tenants")]
    public async Task<IActionResult> ListDemo()
    {
        var items = await dbContext.Tenants.ToListAsync();
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
