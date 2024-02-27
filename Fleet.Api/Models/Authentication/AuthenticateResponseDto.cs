namespace Regira.Fleet.Api.Models.Authentication;

public class AuthenticateResponseDto
{
    public bool IsAuthenticated { get; set; }
    public string? DisplayName { get; set; }
    public string? Token { get; set; }
    public IList<string>? Permissions { get; set; }
}