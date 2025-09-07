using Regira.Entities.Models;

namespace Regira.Fleet.Identity.Models.Tenants.Subscriptions;

public class TenantSubscriptionSearchObject : SearchObject
{
    public string? TenantId { get; set; }
}