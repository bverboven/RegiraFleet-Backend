using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection.ServiceBuilders;
using Regira.Entities.DependencyInjection.ServiceBuilders.Extensions;


namespace Regira.Fleet.Core.DependencyInjection;

public class FleetServiceBuilder<TContext>(IServiceCollection services, FleetHostingOptions options)
    where TContext : DbContext
{
    public FleetHostingOptions Options => options;
    public IServiceCollection Services => services;
    public EntityServiceCollection<TContext> Entities { get; } = services.UseEntities<TContext>();
}
