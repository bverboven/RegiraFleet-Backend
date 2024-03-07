using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.DAL.EFcore.Normalizing;
using Regira.Entities.DependencyInjection;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Entities.EFcore.Abstractions;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Services;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Models;
using Regira.Fleet.Data;
using Regira.Fleet.Entities;
using Regira.Fleet.Entities.Clients;
using Regira.Fleet.Entities.Countries;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.Normalizers;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.Fleet.Normalizing;
using Regira.Fleet.Normalizing.Abstractions;
using Regira.Fleet.Primers;
using Regira.Globalization.LibPhoneNumber;
using Regira.IO.Storage.Abstractions;
using Regira.Normalizing;
using Regira.Normalizing.Abstractions;
using Regira.Normalizing.Models;


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

        // DbContext
        services
            //.AddDbContext<FleetContext>(db => db.UseMySql(options.ConnectionString, ServerVersion.AutoDetect(options.ConnectionString)));
            .AddDbContext<FleetContext>(db =>
            {
                db
                    .UseNpgsql(options.ConnectionString, o =>
                    {
                        o
                            .MigrationsAssembly(typeof(FleetContext).Assembly.GetName().Name)
                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    })
                    //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
#if DEBUG
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
#endif
                    ;
            });

        // Contexts
        services
            .AddScoped<IClientContext, ClientContext>()
            .AddScoped<ICultureContext, CultureContext>()
            .AddScoped<IFleetAppContext, FleetAppContext>();

        // Identity helpers
        services
            .AddHttpContextAccessor()
            .AddTransient<IClientUserClaimsService, IdentityClientUserClaimsService>();

        // Entities
        return services.AddEntities(options);
    }

    static IServiceCollection AddEntities(this IServiceCollection services, FleetHostingOptions options)
    {
        return services
        // Entity context
        .UseEntities<FleetContext>(c => c.ProfileAssemblies.Add(typeof(FleetProfile).Assembly))
        // Entity Items
        // Country
        .For<Country, string, CountryRepository>(e => e.AddMapping<CountryDto, CountryDto>())
        .For<Intervention, InterventionRepository, InterventionSearchObject, EntitySortBy, InterventionIncludes>(e =>
        {
            e.HasRepository<InterventionRepository>();
            e.HasAttachments<FleetContext, Intervention, InterventionAttachment>();
        })
        .For<Brand, BrandRepository, BrandSearchObject, EntitySortBy, EntityIncludes>()
        .For<Vehicle, VehicleRepository, VehicleSearchObject, EntitySortBy, VehicleIncludes>(e =>
        {
            e.HasRepository<VehicleRepository>();
            e.HasAttachments<FleetContext, Vehicle, VehicleAttachment>();
        })
        .For<VehicleType, VehicleTypeRepository, VehicleTypeSearchObject, EntitySortBy, EntityIncludes>()
        .For<InterventionType, InterventionTypeRepository, InterventionTypeSearchObject, EntitySortBy, EntityIncludes>()
        .For<Operator, OperatorRepository, OperatorSearchObject, EntitySortBy, OperatorIncludes>(e =>
        {
            e.HasRepository<OperatorRepository>();
            e.HasAttachments<FleetContext, Operator, OperatorAttachment>();
        })
        // Attachments
        .AddAttachmentServices(options.FileServiceFactory ?? throw new InvalidOperationException($"No implementation for {nameof(IFileService)} configured"))
        // Normalizers
        .AddNormalizers()
        // Primers
        .AddPrimers();
    }
    static EntityServiceCollection<FleetContext> AddAttachmentServices(this EntityServiceCollection<FleetContext> services, Func<IServiceProvider, IFileService> configure)
    {
        return services
            .ConfigureAttachmentService(configure)
            .ConfigureTypedAttachmentService(db => (new[]
            {
                db.InterventionAttachments.ToDescriptor<Intervention>(),
                db.InterventionOperatorAttachments.ToDescriptor<Operator>(),
                db.VehicleAttachments.ToDescriptor<Vehicle>(),
            }));
    }
    static IServiceCollection AddNormalizers(this IServiceCollection services)
    {
        return services
            .AddTransient<INormalizer>(_ => new DefaultNormalizer(new NormalizeOptions { Transform = TextTransform.ToUpperCase }))
            .AddTransient<IObjectNormalizer>(p => new FleetEntityNormalizer(p.GetRequiredService<INormalizer>()))
            // simple normalizers
            .AddTransient<IFleetEntityNormalizer<Brand>, FleetEntityNormalizer<Brand>>()
            .AddTransient<IFleetEntityNormalizer<InterventionType>, FleetEntityNormalizer<InterventionType>>()
            .AddTransient<IFleetEntityNormalizer<VehicleType>, FleetEntityNormalizer<VehicleType>>()
            // helpers
            .AddTransient<AddressNormalizer>()
            .AddTransient(p => new PhoneNumberFormatter(p.GetRequiredService<ICultureContext>().Culture))
            .AddTransient<ContactDataNormalizer>()
            .AddTransient<IdentificationNumberNormalizer>()
            // custom normalizers
            .AddTransient<IFleetEntityNormalizer<Intervention>, InterventionNormalizer>()
            .AddTransient<IFleetEntityNormalizer<Operator>, OperatorNormalizer>()
            .AddTransient<IFleetEntityNormalizer<Vehicle>, VehicleNormalizer>()
            // finally (put last)
            .AddObjectNormalizingContainer((_, c) => c.ExtractFromServiceCollection(services));
    }
    static IServiceCollection AddPrimers(this IServiceCollection services)
    {
        return services
            .AddTransient<IEntityPrimer<IHasCreated>, HasCreatedDbPrimer>()
            .AddTransient<IEntityPrimer<IHasLastModified>, HasLastModifiedDbPrimer>()
            .RegisterPrimerContainer<FleetContext>();
    }
}