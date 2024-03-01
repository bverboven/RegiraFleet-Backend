namespace Regira.Fleet.Identity.Web.Models;

public class ForgotPasswordModel
{
    public string Token { get; set; } = null!;
    public string Username { get; set; } = null!;
}