using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Regira.Fleet.Api.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Entities.Users;
using System.Security.Claims;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("user")]
public class UserController(UserManager<FleetUser> userManager) : ControllerBase
{
    [HttpPost("personal-data")]
    public async Task<IActionResult> ChangePersonalData(ChangePersonalDataInput model)
    {
        var item = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (item == null)
        {
            return NotFound();
        }

        // Culture
        if (item.Culture != model.Culture)
        {
            item.Culture = model.Culture;
            await userManager.UpdateAsync(item);
        }

        // Claims
        var claims = await userManager.GetClaimsAsync(item);
        var givenNameClaims = claims.Where(x => x.Type == FleetClaimTypes.GivenName);
        var lastNameClaims = claims.Where(x => x.Type == FleetClaimTypes.LastName);

        var claimsToRemove = new List<Claim>(givenNameClaims.Skip(1).Concat(lastNameClaims.Skip(1)));
        var claimsToAdd = new List<Claim>();

        var givenNameClaim = givenNameClaims.FirstOrDefault();
        var lastNameClaim = lastNameClaims.FirstOrDefault();

        // Given name
        if (!string.IsNullOrWhiteSpace(model.GivenName))
        {
            var claim = new Claim(FleetClaimTypes.GivenName, model.GivenName);
            if (givenNameClaim == null)
            {
                claimsToAdd.Add(claim);
            }
            else if (givenNameClaim.Value != model.GivenName)
            {
                await userManager.ReplaceClaimAsync(item, givenNameClaim, claim);
            }
        }
        else
        {
            if (givenNameClaim != null)
            {
                claimsToRemove.Add(givenNameClaim);
            }
        }
        // Last name
        if (!string.IsNullOrWhiteSpace(model.LastName))
        {
            var claim = new Claim(FleetClaimTypes.LastName, model.LastName);
            if (lastNameClaim == null)
            {
                claimsToAdd.Add(claim);
            }
            else if (lastNameClaim.Value != model.LastName)
            {
                await userManager.ReplaceClaimAsync(item, lastNameClaim, claim);
            }
        }
        else
        {
            if (lastNameClaim != null)
            {
                claimsToRemove.Add(lastNameClaim);
            }
        }

        if (claimsToRemove.Any())
        {
            await userManager.RemoveClaimsAsync(item, claimsToRemove);
        }
        if (claimsToAdd.Any())
        {
            await userManager.AddClaimsAsync(item, claimsToAdd);
        }


        return Ok();
    }


    [Authorize(FleetPolicies.AdminPolicy)]
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        return Ok("ToDo");
    }
}
