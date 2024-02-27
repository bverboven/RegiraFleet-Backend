namespace Regira.Fleet.Api.Models.Authentication;

public class AuthenticateInputDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}