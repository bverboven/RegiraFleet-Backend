using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Entities.Users.Claims;

public class ClientUserClaimDto
{
    public int Id { get; set; }
    [StringLength(32)]
    public string ClientId { get; set; } = null!;
    public string ClaimType { get; set; } = null!;
    public string? ClaimValue { get; set; }
}