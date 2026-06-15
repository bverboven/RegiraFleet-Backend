using Regira.Entities.Models;

namespace Regira.Fleet.Identity.Models.Users;

public record FleetUserSearchObject : SearchObject<string>
{
    public string? UserName { get; set; }
    public string? TenantId { get; set; }
    public string? Title { get; set; }
    public string? Culture { get; set; }
    public ICollection<string>? Permissions { get; set; }
}
