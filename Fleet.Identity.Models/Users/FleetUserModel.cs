using Microsoft.AspNetCore.Identity;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Identity.Models.Tenants;
using Regira.Fleet.Identity.Models.Users.Claims;

namespace Regira.Fleet.Identity.Models.Users;

public class FleetUserModel : IEntity<string>
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }

    public string? GivenName { get; set; }
    public string? LastName { get; set; }
    public string? Culture { get; set; }

    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }

    public ICollection<IdentityUserClaim<string>>? UserClaims { get; set; }
    public ICollection<TenantUserClaim>? TenantClaims { get; set; }
    public ICollection<Tenant>? Tenants => TenantClaims
        ?.Where(c => c.Tenant != null)
        .Select(c => c.Tenant!)
        .DistinctBy(c => c.Id)
        .ToList();
}