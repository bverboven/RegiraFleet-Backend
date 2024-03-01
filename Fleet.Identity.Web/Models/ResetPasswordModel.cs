namespace Regira.Fleet.Identity.Web.Models;

public class ResetPasswordModel
{
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
}