using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Tenants;

public class TenantContext(IHttpContextAccessor httpContextAccessor) : ITenantContext
{
    public string? TenantId => httpContextAccessor.HttpContext?.User.FindFirstValue(FleetClaimTypes.TenantId);
}