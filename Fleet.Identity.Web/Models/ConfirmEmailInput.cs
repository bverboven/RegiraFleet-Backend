using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Web.Models;

public class ConfirmEmailInput
{
    [Required]
    public string Token { get; set; } = null!;
    public string? Password { get; set; }
}