using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Regira.Fleet.Admin.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("cultures")]
public class CultureController : ControllerBase
{
    [HttpGet]
    public IActionResult GetCultures()
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures & ~CultureTypes.NeutralCultures)
            .Where(c => c.LCID != 4096)
            .ToArray();
        return Ok(cultures
            .Select(c => c.Name)
            .OrderBy(c => c));
    }
}
