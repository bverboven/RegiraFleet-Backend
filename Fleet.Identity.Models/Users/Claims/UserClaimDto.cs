namespace Regira.Fleet.Identity.Models.Users.Claims;

public class UserClaimDto
{
    public int Id { get; set; }
    public string ClaimType { get; set; } = null!;
    public string? ClaimValue { get; set; }
}
