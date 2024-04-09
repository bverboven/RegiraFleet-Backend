using Microsoft.Extensions.DependencyInjection;
using Regira.Fleet.Core.DependencyInjection;
using Regira.Fleet.Identity.Data;


namespace Regira.Fleet.Identity.DependencyInjection;

public class FleetServiceBuilder(IServiceCollection services, FleetHostingOptions options) : FleetServiceBuilder<AccountsContextBase>(services, options)
{
}
