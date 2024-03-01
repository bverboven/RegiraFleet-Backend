namespace Regira.Fleet.Api.Models.Authentication;

public class AuthenticateResponseDto
{
    public bool IsAuthenticated { get; set; }
    public string? Token { get; set; }

    public bool? IsLockedOut { get; set; }
    public DateTime? LockedOutEnd { get; set; }
}