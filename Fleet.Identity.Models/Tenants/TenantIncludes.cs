namespace Regira.Fleet.Identity.Models.Tenants;

[Flags]
public enum TenantIncludes
{
    None = 0,
    Languages = 1 << 0,
    Subscriptions = 1 << 1,
    ActiveSubscription = 1 << 2,
    All = Languages | Subscriptions
}
