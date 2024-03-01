namespace Regira.Fleet.Identity.Web.Models;

public class RecoverPasswordInput
{
    public string Username { get; set; } = null!;
    public string SiteUrl { get; set; } = null!;
    public string? SiteName { get; set; }
}