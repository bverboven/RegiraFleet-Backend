using Regira.Fleet.Identity.DependencyInjection;

namespace Regira.Fleet.Identity.Web.DependencyInjection;

public class FleetIdentityOptions : FleetAuthenticationOptions
{
    public string? SecretKey { get; set; }
    public List<string> Audiences { get; } = [];
}
