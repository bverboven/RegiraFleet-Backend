using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Tenants;

public class WritableTenantContext : ITenantContext
{
    public string? TenantId { get; set; }
}