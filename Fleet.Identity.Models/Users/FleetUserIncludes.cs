namespace Regira.Fleet.Identity.Models.Users;

[Flags]
public enum FleetUserIncludes
{
    None = 0,
    UserClaims = 1 << 0,
    TenantClaims = 1 << 1,
    Claims = UserClaims | TenantClaims,
    All = Claims
}
