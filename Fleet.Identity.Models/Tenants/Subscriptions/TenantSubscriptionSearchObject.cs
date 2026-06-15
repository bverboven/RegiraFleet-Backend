using Regira.Entities.Models;

namespace Regira.Fleet.Identity.Models.Tenants.Subscriptions;

public record TenantSubscriptionSearchObject : SearchObject
{
    public string? TenantId { get; set; }
}