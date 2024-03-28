using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Entities.Users.Claims;
using System.Security.Claims;

namespace Regira.Fleet.Identity.Services;

/// <summary>
/// Add ClientUserClaims (corresponding to selected client) to the identity claims.
/// Requests "clientId" from QueryString
/// </summary>
/// <param name="dbContext"></param>
/// <param name="httpContextAccessor"></param>
public class IdentityClientUserClaimsService(AccountsContext dbContext, IHttpContextAccessor httpContextAccessor) : IClientUserClaimsService
{
    public async Task Process(ClaimsIdentity identity)
    {
        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var requestedClientId = httpContextAccessor.HttpContext?.Request.Query["clientId"].ToString();
        if (string.IsNullOrWhiteSpace(requestedClientId))
        {
            var firstUserClient = await dbContext.Clients.FirstOrDefaultAsync(c => c.UserClaims!.Any(c => c.UserId == userId));
            requestedClientId = firstUserClient?.Id;
        }

        var claims = await GetClaims(userId, requestedClientId);
        if (!string.IsNullOrWhiteSpace(requestedClientId) && claims.Any(c => c.ClientId == requestedClientId))
        {
            identity.AddClaim(new Claim(FleetClaimTypes.ClientId, requestedClientId));
        }
        foreach (var claim in claims)
        {
            identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue ?? string.Empty));
        }

        // remove unused clientIds from IdentityClaims
        //var clientClaimsToRemove = identity.Claims
        //    .Where(c => c.Type == FleetClaimTypes.ClientId && c.Value != requestedClientId)
        //    .ToArray();
        //foreach (var claim in clientClaimsToRemove)
        //{
        //    identity.RemoveClaim(claim);
        //}
    }

    Task<List<ClientUserClaim>> GetClaims(string userId, string? clientId)
        => dbContext.Clients
            .Where(c => c.Id == clientId)
            .SelectMany(c => c.UserClaims!.Where(uc => uc.UserId == userId))
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();
}
