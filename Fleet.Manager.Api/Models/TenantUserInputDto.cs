using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Manager.Api.Models;

public class TenantUserInputDto
{
    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = null!;
    [MaxLength(8)]
    public string? Culture { get; set; }
    public string? SiteUrl { get; set; }

    public ICollection<string>? Permissions { get; set; }
}
