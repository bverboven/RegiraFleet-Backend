using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models.Users.Claims;

namespace Regira.Fleet.Identity.Services;

/// <summary>
/// Add TenantUserClaims (corresponding to selected tenant) to the identity claims.
/// Requests "tenantId" from QueryString
/// </summary>
/// <param name="dbContext"></param>
/// <param name="httpContextAccessor"></param>
public class IdentityTenantUserClaimsService(AccountsContextBase dbContext, IHttpContextAccessor httpContextAccessor) : ITenantUserClaimsService
{
    public async Task Process(ClaimsIdentity identity)
    {
        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var requestedTenantId = httpContextAccessor.HttpContext?.Request.Query["tenantId"].ToString();

        var allClaims = await GetTenantClaims(userId);
        var tenantClaims = allClaims.FindAll(x => x.TenantId == requestedTenantId);
        if (!tenantClaims.Any())
        {
            // return claims for first tenant if requestedTenant is not present
            tenantClaims = allClaims
                .GroupBy(x => x.TenantId)
                .SelectMany(x => x.ToList())
                .ToList();
        }
        if (tenantClaims.Any())
        {
            identity.AddClaim(new Claim(FleetClaimTypes.TenantId, tenantClaims.First().TenantId));
        }
        foreach (var claim in tenantClaims)
        {
            identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue ?? string.Empty));
        }
    }

    Task<List<TenantUserClaim>> GetTenantClaims(string userId)
        => dbContext.TenantUserClaims
            .Where(u => u.UserId == userId)
            .ToListAsync();
}
