namespace Regira.Fleet.Identity.Models.Clients.Subscriptions;

public class ClientSubscriptionDto
{
    public int Id { get; set; }
    public string ClientId { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}
