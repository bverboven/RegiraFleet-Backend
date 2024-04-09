using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Models.Clients.Subscriptions;

public class ClientSubscriptionInputDto
{
    public int Id { get; set; }
    public string ClientId { get; set; } = null!;
    [Required]
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}