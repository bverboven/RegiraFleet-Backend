using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Identity.Models.Tenants;

namespace Regira.Fleet.Identity.Models.Users.Claims;

public class TenantUserClaim : IEntityWithSerial, IHasTenantId, IHasUserId
{
    public int Id { get; set; }
    [StringLength(32)]
    public string TenantId { get; set; } = null!;
    [Required]
    [MaxLength(64)]
    public string? UserId { get; set; }
    [MaxLength(64)]
    public string ClaimType { get; set; } = null!;
    [MaxLength(256)]
    public string? ClaimValue { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;

    public Tenant? Tenant { get; set; }
    public FleetUser? User { get; set; }
}
