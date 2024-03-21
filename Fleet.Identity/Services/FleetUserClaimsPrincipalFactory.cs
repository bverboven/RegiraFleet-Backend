using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Entities.Users;
using System.Security.Claims;

namespace Regira.Fleet.Identity.Services;

public class FleetUserClaimsPrincipalFactory(FleetUserIdentityManager userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options, IEnumerable<IClientUserClaimsService> clientUserClaimsService)
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

        var displayName = $"{identity.FindFirst(FleetClaimTypes.GivenName)?.Value} {identity.FindFirst(FleetClaimTypes.LastName)?.Value}".Trim();
        if (!string.IsNullOrWhiteSpace(displayName))
        {
            identity.AddClaim(new Claim(FleetClaimTypes.DisplayName, $"{displayName}"));
        }

        var isSuperUser = identity.HasClaim(c => c.Type == identity.RoleClaimType && c.Value == FleetClaimTypes.SuperUser);
        if (isSuperUser)
        {
            identity.AddClaim(new Claim(FleetClaimTypes.Permission, FleetClaimTypes.SuperUser));
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