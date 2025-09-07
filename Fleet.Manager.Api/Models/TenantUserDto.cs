namespace Regira.Fleet.Manager.Api.Models;

public class TenantUserDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? DisplayName { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool HasPassword { get; set; }
    public ICollection<string> Permissions { get; set; } = null!;
}
