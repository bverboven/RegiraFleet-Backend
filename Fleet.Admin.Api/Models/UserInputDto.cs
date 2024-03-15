using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Admin.Api.Models;

public class UserInputDto
{
    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = null!;
    public string? Password { get; set; }
    [MaxLength(8)]
    public string? Culture { get; set; }
}