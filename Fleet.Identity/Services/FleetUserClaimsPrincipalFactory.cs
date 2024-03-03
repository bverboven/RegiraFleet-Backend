using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Models;
using System.Security.Claims;

namespace Regira.Fleet.Identity.Services;

public class FleetUserClaimsPrincipalFactory(FleetUserManager userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options, IEnumerable<IClientUserClaimsService> clientUserClaimsService)
    : UserClaimsPrincipalFactory<FleetUser, IdentityRole>(userManager, roleManager, options)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(FleetUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        // culture
        if (!string.IsNullOrWhiteSpace(user.Culture))
        {
            identity.AddClaim(new Claim(FleetClaimTypes.Culture, user.Culture));
        }
        // ui_culture
        //if (!string.IsNullOrWhiteSpace(user.UICulture))
        //{
        //    identity.AddClaim(new Claim(FleetClaimTypes.UICulture, user.UICulture));
        //}

        foreach (var claimService in clientUserClaimsService)
        {
            await claimService.Process(identity);
        }


        return identity;
    }
}