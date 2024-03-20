using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Clients.Subscriptions;

public class ClientSubscriptionInputDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    [Required]
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}