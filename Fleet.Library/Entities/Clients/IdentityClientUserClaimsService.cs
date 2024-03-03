using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Data;
using System.Security.Claims;

namespace Regira.Fleet.Entities.Clients;

/// <summary>
/// Add ClientUserClaims (corresponding to selected client) to the identity claims
/// </summary>
/// <param name="dbContext"></param>
/// <param name="httpContextAccessor"></param>
public class IdentityClientUserClaimsService(FleetContext dbContext, IHttpContextAccessor httpContextAccessor) : IClientUserClaimsService
{
    public async Task Process(ClaimsIdentity identity)
    {
        var requestedClientId = httpContextAccessor.HttpContext?.Request.Query["clientId"];
        var clientId = identity.FindFirst(c => c.Type == FleetClaimTypes.ClientId)?.Value;
        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var claims = await dbContext.Clients
            .Where(c => c.Guid == clientId)
            .SelectMany(c => c.UserClaims!.Where(uc => uc.UserId == userId))
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();

        foreach (var claim in claims)
        {
            identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue ?? string.Empty));
        }
    }
}
