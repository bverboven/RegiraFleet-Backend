namespace Regira.Fleet.Identity.Models.Tenants.Subscriptions;

public class TenantSubscriptionDto
{
    public int Id { get; set; }
    public string TenantId { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}
