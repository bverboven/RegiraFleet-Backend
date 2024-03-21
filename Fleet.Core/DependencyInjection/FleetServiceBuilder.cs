using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection;
using Regira.Entities.DependencyInjection.Extensions;


namespace Regira.Fleet.Core.DependencyInjection;

public class FleetServiceBuilder<TContext>(IServiceCollection services, FleetHostingOptions options)
    where TContext : DbContext
{
    protected internal FleetHostingOptions Options => options;
    public IServiceCollection Services => services;
    public EntityServiceCollection<TContext> Entities { get; } = services.UseEntities<TContext>();
}
