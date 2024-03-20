using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Data;
using System.Security.Claims;

namespace Regira.Fleet.Entities.Clients.Users;

/// <summary>
/// Add ClientUserClaims (corresponding to selected client) to the identity claims.
/// Requests "clientId" from QueryString
/// </summary>
/// <param name="dbContext"></param>
/// <param name="httpContextAccessor"></param>
public class IdentityClientUserClaimsService(FleetContext dbContext, IHttpContextAccessor httpContextAccessor) : IClientUserClaimsService
{
    public async Task Process(ClaimsIdentity identity)
    {
        var requestedClientId = httpContextAccessor.HttpContext?.Request.Query["clientId"].ToString();
        var clientId = identity.FindFirst(c => c.Type == FleetClaimTypes.ClientId)?.Value;
        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var claims = await GetClaims(userId, clientId);
        foreach (var claim in claims)
        {
            identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue ?? string.Empty));
        }

        // remove unused clientIds from IdentityClaims
        var clientClaimsToRemove = identity.Claims
            .Where(c => c.Type == FleetClaimTypes.ClientId && c.Value != requestedClientId)
            .ToArray();
        foreach (var claim in clientClaimsToRemove)
        {
            identity.RemoveClaim(claim);
        }
    }

    Task<List<ClientUserClaim>> GetClaims(string userId, string? clientId)
        => dbContext.Clients
            .Where(c => c.Guid == clientId)
            .SelectMany(c => c.UserClaims!.Where(uc => uc.UserId == userId))
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();
}
