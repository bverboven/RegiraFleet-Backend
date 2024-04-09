using Microsoft.Extensions.DependencyInjection;
using Regira.Fleet.Core.DependencyInjection;
using Regira.Fleet.Data;


namespace Regira.Fleet.DependencyInjection;

public class FleetServiceBuilder(IServiceCollection services, FleetHostingOptions options) : FleetServiceBuilder<FleetContextBase>(services, options)
{
}
