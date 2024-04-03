using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Manager.Api.Models;

public class RequestConfirmEmailInput
{
    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = null!;
    [Required]
    public string SiteUrl { get; set; } = null!;
}
