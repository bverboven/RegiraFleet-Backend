using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Models.Users.Claims;

public class TenantUserClaimDto
{
    public int Id { get; set; }
    [StringLength(32)]
    public string TenantId { get; set; } = null!;
    public string ClaimType { get; set; } = null!;
    public string? ClaimValue { get; set; }
}