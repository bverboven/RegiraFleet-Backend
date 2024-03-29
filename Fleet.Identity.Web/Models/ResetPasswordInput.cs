namespace Regira.Fleet.Identity.Web.Models;

public class ResetPasswordInput
{
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
}
