using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Web.Models;

public class ChangePasswordInput
{
    [Required]
    public string NewPassword { get; set; } = null!;
    [Required]
    public string CurrentPassword { get; set; } = null!;
}