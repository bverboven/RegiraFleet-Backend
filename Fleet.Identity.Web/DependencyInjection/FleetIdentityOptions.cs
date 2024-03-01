using static Regira.Fleet.Identity.DependencyInjection.ServiceCollectionExtensions;

namespace Regira.CRM.Identity.Web.DependencyInjection;

public class FleetIdentityOptions : FleetAuthenticateOptions
{
    public string? SecretKey { get; set; }
    public List<string> Audiences { get; } = new();
}
