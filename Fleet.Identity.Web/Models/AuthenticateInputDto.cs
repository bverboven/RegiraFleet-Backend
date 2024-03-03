using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Web.Models;

public class AuthenticateInputDto
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}