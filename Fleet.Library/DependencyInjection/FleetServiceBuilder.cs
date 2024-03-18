using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Fleet.Data;


namespace Regira.Fleet.DependencyInjection;

public class FleetServiceBuilder(IServiceCollection services, FleetHostingOptions options)
{
    protected internal FleetHostingOptions Options => options;
    public IServiceCollection Services => services;
    public EntityServiceCollection<FleetContext> Entities { get; } = services.UseEntities<FleetContext>();
}
