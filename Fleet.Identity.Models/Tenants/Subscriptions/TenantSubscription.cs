using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Identity.Models.Tenants.Subscriptions;

public class TenantSubscription : IEntityWithSerial, IHasTenantId, IHasDescription, IHasStartEndDate, IHasTimestamps
{
    public int Id { get; set; }
    [StringLength(32)]
    public string TenantId { get; set; } = null!;
    [Required]
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string? Description { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }


    public Tenant? Tenant { get; set; }
}
