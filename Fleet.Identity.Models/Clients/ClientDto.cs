using Regira.Fleet.Identity.Models.Clients.Subscriptions;

namespace Regira.Fleet.Identity.Models.Clients;

public class ClientDto
{
    public string Id { get; set; } = null!;
    public string? Code { get; set; }
    public string Title { get; set; } = null!;
    public string? DefaultCulture { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }

    public string? Description { get; set; }

    public ICollection<string>? Languages { get; set; }
    public ICollection<ClientSubscriptionDto>? Subscriptions { get; set; }
}
