namespace Regira.Fleet.Authentication;

public class FleetUser
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string[] Permissions { get; set; } = null!;
    public string? DisplayName { get; set; }
}