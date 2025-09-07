using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Models.Tenants.Subscriptions;

public class TenantSubscriptionInputDto
{
    public int Id { get; set; }
    public string TenantId { get; set; } = null!;
    [Required]
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}