using Regira.Entities.Models;

namespace Regira.Fleet.Identity.Models.Users;

public class FleetUserSearchObject : SearchObject<string>
{
    public string? UserName { get; set; }
    public string? ClientId { get; set; }
    public string? Name { get; set; }
    public ICollection<string>? Permissions { get; set; }
}
