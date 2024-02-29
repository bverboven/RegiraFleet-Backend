using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.Models;
using Regira.Fleet.Data;
using Regira.Fleet.Entities;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Suppliers;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.IO.Storage.Abstractions;

namespace Regira.Fleet.DependencyInjection;

public class FleetHostingOptions
{
    public string? ConnectionString { get; set; }
    internal Func<IServiceProvider, IFileService>? FileServiceFactory { get; private set; }
    public void ConfigureStorageService(Func<IServiceProvider, IFileService> configure) => FileServiceFactory = configure;
}
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFleet(this IServiceCollection services, Action<FleetHostingOptions> configure)
    {
        var options = new FleetHostingOptions();
        configure.Invoke(options);

        services
            //.AddDbContext<FleetContext>(db => db.UseMySql(options.ConnectionString, ServerVersion.AutoDetect(options.ConnectionString)));
            .AddDbContext<FleetContext>(db => db.UseNpgsql(options.ConnectionString, o => o.MigrationsAssembly(typeof(FleetContext).Assembly.GetName().Name)));

        services
            //.AddAutoMapper(typeof(FleetProfile).Assembly)
            .UseEntities<FleetContext>(c => c.ProfileAssemblies.Add(typeof(FleetProfile).Assembly))
            .AddAttachmentServices(options.FileServiceFactory ?? throw new InvalidOperationException($"No implementation for {nameof(IFileService)} configured"))
            .For<Intervention, InterventionRepository, InterventionSearchObject, EntitySortBy, InterventionIncludes>(e =>
            {
                e.HasRepository<InterventionRepository>();
                e.HasAttachments<FleetContext, Intervention, InterventionAttachment>();
            })
            .For<Brand, BrandRepository, BrandSearchObject, EntitySortBy, EntityIncludes>()
            .For<Vehicle, VehicleRepository, VehicleSearchObject, EntitySortBy, EntityIncludes>(e =>
            {
                e.HasRepository<VehicleRepository>();
                e.HasAttachments<FleetContext, Vehicle, VehicleAttachment>();
            })
            .For<VehicleType, VehicleTypeRepository, VehicleTypeSearchObject, EntitySortBy, EntityIncludes>()
            .For<InterventionType, InterventionTypeRepository, InterventionTypeSearchObject, EntitySortBy, EntityIncludes>()
            .For<Supplier, SupplierRepository, SupplierSearchObject, EntitySortBy, SupplierIncludes>(e =>
            {
                e.HasRepository<SupplierRepository>();
                e.HasAttachments<FleetContext, Supplier, SupplierAttachment>();
            });

        return services;
    }

    static EntityServiceCollection<FleetContext> AddAttachmentServices(this EntityServiceCollection<FleetContext> services, Func<IServiceProvider, IFileService> configure)
    {
        return services
            .ConfigureAttachmentService(configure)
            .ConfigureTypedAttachmentService(db => new[]
            {
                db.CarAttachments.ToDescriptor<Vehicle>(),
            });
    }
}