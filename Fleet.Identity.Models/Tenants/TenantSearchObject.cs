using Regira.Entities.Models;

namespace Regira.Fleet.Identity.Models.Tenants;

public record TenantSearchObject : SearchObject<string>
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Culture { get; set; }
}