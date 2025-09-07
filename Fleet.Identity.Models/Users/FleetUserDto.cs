using Regira.Fleet.Identity.Models.Tenants;
using Regira.Fleet.Identity.Models.Users.Claims;

namespace Regira.Fleet.Identity.Models.Users;

public class FleetUserDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }

    public string? GivenName { get; set; }
    public string? LastName { get; set; }
    public string? Culture { get; set; }

    public ICollection<UserClaimDto>? UserClaims { get; set; }
    public ICollection<TenantUserClaimDto>? TenantClaims { get; set; }
    public ICollection<TenantDto>? Tenants { get; set; }
}
