namespace Regira.Fleet.Identity.Entities.Clients;

[Flags]
public enum ClientIncludes
{
    None = 0,
    Languages = 1 << 0,
    Subscriptions = 1 << 1,
    ActiveSubscription = 1 << 2,
    All = Languages | Subscriptions
}
