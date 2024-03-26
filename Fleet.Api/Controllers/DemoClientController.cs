using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Data;

namespace Regira.Fleet.Api.Controllers;

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
