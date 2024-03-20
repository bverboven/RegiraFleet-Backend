namespace Regira.Fleet.Entities.Clients.Subscriptions;

public class ClientSubscriptionDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }

    public ClientDto? Client { get; set; }
}
