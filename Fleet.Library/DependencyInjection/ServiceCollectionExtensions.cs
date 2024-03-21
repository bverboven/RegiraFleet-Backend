using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.DAL.EFcore.Normalizing;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Entities.EFcore.Abstractions;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Services;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Clients;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Models;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Core.Normalizing.Abstractions;
using Regira.Fleet.Core.Primers;
using Regira.Fleet.Data;
using Regira.Fleet.Entities;
using Regira.Fleet.Entities.Countries;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.Normalizers;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.Globalization.LibPhoneNumber;
using Regira.IO.Storage.Abstractions;
using Regira.Normalizing;
using Regira.Normalizing.Abstractions;
using Regira.Normalizing.Models;


namespace Regira.Fleet.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static FleetServiceBuilder AddFleet(this IServiceCollection services, Action<FleetHostingOptions> configure)
    {
        var options = new FleetHostingOptions();
        configure.Invoke(options);

        var fleetBuilder = new FleetServiceBuilder(services, options)
            // Database context
            .AddDbContext(options.ConnectionString!)
            // Contexts
            .AddAppContexts()
            // Identity helpers
            .AddIdentityHelpers()
            // Entities
            .AddFleetEntities(options)
            // Attachments
            .AddAttachmentServices(options.FileServiceFactory ?? throw new InvalidOperationException($"No implementation for {nameof(IFileService)} configured"));

        return fleetBuilder;
    }


    public static FleetServiceBuilder AddDbContext(this FleetServiceBuilder builder, string connectionString)
    {
        builder.Services
             //.AddDbContext<FleetContext>(db => db.UseMySql(options.ConnectionString, ServerVersion.AutoDetect(options.ConnectionString)));
             .AddDbContext<FleetContext>(db =>
             {
                 db
                     .UseNpgsql(connectionString, o =>
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

        return builder;
    }
    public static FleetServiceBuilder AddIdentityHelpers(this FleetServiceBuilder builder)
    {
        builder.Services
              .AddHttpContextAccessor()
              //.AddTransient<IClientUserClaimsService, IdentityClientUserClaimsService>()
              ;

        return builder;
    }
    public static FleetServiceBuilder AddAppContexts(this FleetServiceBuilder builder)
    {
        builder.Services
              .AddScoped<IClientContext, ClientContext>()
              .AddScoped<ICultureContext, CultureContext>()
              .AddScoped<IFleetAppContext, FleetAppContext>();

        return builder;
    }
    public static FleetServiceBuilder AddFleetEntities(this FleetServiceBuilder builder, FleetHostingOptions options)
    {
        builder.Services
            //Entity context
            .UseEntities<FleetContext>(c => c.ProfileAssemblies.Add(typeof(FleetProfile).Assembly));

        // Entity Items
        builder.Entities
           // Country
           .For<Country, string, CountryRepository>(e => e.AddMapping<CountryDto, CountryDto>())
           .For<Intervention, InterventionRepository, InterventionSearchObject, InterventionSortBy, InterventionIncludes>(e =>
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
           });

        builder
           // Normalizers
           .AddNormalizers(o =>
           {
               o
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
                   .AddTransient<IFleetEntityNormalizer<Vehicle>, VehicleNormalizer>();
           })
           // Primers
           .AddPrimers();

        return builder;
    }
    public static FleetServiceBuilder AddAttachmentServices(this FleetServiceBuilder builder, Func<IServiceProvider, IFileService> configure)
    {
        builder.Entities
            .ConfigureAttachmentService(configure)
            .ConfigureTypedAttachmentService(db => (new[]
            {
                db.InterventionAttachments.ToDescriptor<Intervention>(),
                db.InterventionOperatorAttachments.ToDescriptor<Operator>(),
                db.VehicleAttachments.ToDescriptor<Vehicle>(),
            }));

        return builder;
    }
    public static FleetServiceBuilder AddNormalizers(this FleetServiceBuilder builder, Action<IServiceCollection> configure)
    {
        builder.Services
            .AddTransient<INormalizer>(_ => new DefaultNormalizer(new NormalizeOptions { Transform = TextTransform.ToUpperCase }))
            .AddTransient<IObjectNormalizer>(p => new FleetEntityNormalizer(p.GetRequiredService<INormalizer>()));

        // configure entity normalizers
        configure?.Invoke(builder.Services);

        // finally (put last)
        builder.Services
                    .AddObjectNormalizingContainer((_, c) => c.ExtractFromServiceCollection(builder.Services));

        return builder;
    }
    public static FleetServiceBuilder AddPrimers(this FleetServiceBuilder builder)
    {
        builder.Services
            .AddTransient<IEntityPrimer<IHasCreated>, HasCreatedDbPrimer>()
            .AddTransient<IEntityPrimer<IHasLastModified>, HasLastModifiedDbPrimer>()
            .RegisterPrimerContainer<FleetContext>();

        return builder;
    }
}