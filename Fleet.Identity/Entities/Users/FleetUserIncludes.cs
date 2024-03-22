namespace Regira.Fleet.Identity.Entities.Users;

[Flags]
public enum FleetUserIncludes
{
    None = 0,
    UserClaims = 1 << 0,
    ClientClaims = 1 << 1,
    Claims = UserClaims | ClientClaims,
    All = Claims
}
