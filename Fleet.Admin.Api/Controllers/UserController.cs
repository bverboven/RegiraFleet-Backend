using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Regira.DAL.Paging;
using Regira.Fleet.Admin.Api.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Data;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models;
using Regira.Fleet.Identity.Services;
using Regira.Fleet.Identity.Web.Extensions;
using Regira.Fleet.Identity.Web.Models;
using System.Security.Claims;

namespace Regira.Fleet.Admin.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("users")]
public class UserController(AccountsContext dbContext, FleetContext fleetContext, FleetUserManager userManager, IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Details([FromRoute] string id)
    {
        var item = await dbContext.Users
            .Include(x => x.UserClaims)
            .Include(x => x.UserRoles)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        var dto = mapper.Map<FleetUserDto>(item);
        return Ok(dto);
    }
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] UserSearchObject so, [FromQuery] PagingInfo pagingInfo)
    {
        IQueryable<FleetUser> query = dbContext.Users
            .Include(x => x.UserClaims);
        query = query.PageQuery(pagingInfo);
        var models = await query.ToListAsync();
        var items = mapper.Map<List<FleetUserDto>>(models);
        return Ok(items);
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] UserInputDto input)
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
                claims.Add(new Claim(FleetClaimTypes.GivenName, input.GivenName));
            }
            if (!string.IsNullOrWhiteSpace(input.LastName))
            {
                claims.Add(new Claim(FleetClaimTypes.LastName, input.LastName));
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

        var client = await fleetContext.Clients.FindAsync(input.ClientId);
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

public class UserSearchObject
{
    public string? Id { get; set; }
    public ICollection<string>? Ids { get; set; }
    public string? UserName { get; set; }
    public string? Client { get; set; }
    public string? Name { get; set; }
    public ICollection<string>? Permissions { get; set; }
    public string? Q { get; set; }
}