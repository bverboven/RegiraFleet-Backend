using Regira.Entities.Models;

namespace Regira.Fleet.Identity.Models.Clients;

public class ClientSearchObject : SearchObject<string>
{
    public string? Title { get; set; }
    public string? Culture { get; set; }
}