namespace UserManager.Console;

public record AdminUser
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Culture { get; set; } = "nl-BE";
}
