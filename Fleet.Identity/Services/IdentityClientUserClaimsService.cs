using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models.Users.Claims;

namespace Regira.Fleet.Identity.Services;

/// <summary>
/// Add ClientUserClaims (corresponding to selected client) to the identity claims.
/// Requests "clientId" from QueryString
/// </summary>
/// <param name="dbContext"></param>
/// <param name="httpContextAccessor"></param>
public class IdentityClientUserClaimsService(AccountsContextBase dbContext, IHttpContextAccessor httpContextAccessor) : IClientUserClaimsService
{
    public async Task Process(ClaimsIdentity identity)
    {
        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var requestedClientId = httpContextAccessor.HttpContext?.Request.Query["clientId"].ToString();

        var allClaims = await GetClientClaims(userId);
        var clientClaims = allClaims.FindAll(x => x.ClientId == requestedClientId);
        if (!clientClaims.Any())
        {
            // return claims for first client if requestedclient is not present
            clientClaims = allClaims
                .GroupBy(x => x.ClientId)
                .SelectMany(x => x.ToList())
                .ToList();
        }
        if (clientClaims.Any())
        {
            identity.AddClaim(new Claim(FleetClaimTypes.ClientId, clientClaims.First().ClientId));
        }
        foreach (var claim in clientClaims)
        {
            identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue ?? string.Empty));
        }
    }

    Task<List<ClientUserClaim>> GetClientClaims(string userId)
        => dbContext.ClientUserClaims
            .Where(u => u.UserId == userId)
            .ToListAsync();
}
