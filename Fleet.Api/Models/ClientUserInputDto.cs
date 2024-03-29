using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Api.Models;

public class ClientUserInputDto
{
    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = null!;
    [MaxLength(8)]
    public string? Culture { get; set; }
    [Required]
    public string SiteUrl { get; set; } = null!;

    public ICollection<string>? Permissions { get; set; }
}
