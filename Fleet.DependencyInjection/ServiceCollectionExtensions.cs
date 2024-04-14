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
using Regira.Fleet.Core.DependencyInjection;
using Regira.Fleet.Core.Models;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Core.Normalizing.Abstractions;
using Regira.Fleet.Core.Primers;
using Regira.Fleet.Data;
using Regira.Fleet.Data.MySQL;
using Regira.Fleet.Data.PostgreSQL;
using Regira.Fleet.Data.SqlServer;
using Regira.Fleet.Entities.Countries;
using Regira.Fleet.Entities.EntityLabels;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.Normalizers;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.Fleet.Models;
using Regira.Fleet.Models.Countries;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;
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

        var fleetBuilder = new FleetServiceBuilder(services, options);
        fleetBuilder
            // Database context
            .AddDbContext();

        fleetBuilder
            // Contexts
            .AddAppContexts()
            // Identity helpers
            .AddIdentityHelpers();

        fleetBuilder
            // Entities
            .AddFleetEntities(options);

        // Attachments
        fleetBuilder
            .AddAttachmentServices(options.FileServiceFactory ?? throw new InvalidOperationException($"No implementation for {nameof(IFileService)} configured"));

        return fleetBuilder;
    }


    public static FleetServiceBuilder AddDbContext(this FleetServiceBuilder builder)
    {
        return builder.Options.DatabaseType switch
        {
            "PostgreSQL" => builder.AddPgContext(builder.Options.ConnectionString),
            "MySQL" => builder.AddMySqlContext(builder.Options.ConnectionString),
            "SqlServer" => builder.AddSqlServerContext(builder.Options.ConnectionString),
            _ => throw new NotSupportedException($"Type {builder.Options.DatabaseType} not supported"),
        };
    }
    public static FleetServiceBuilder AddDbContext<TContext>(this FleetServiceBuilder builder, Action<DbContextOptionsBuilder> configureDb)
        where TContext : FleetContextBase
    {
        builder.Services
            .AddDbContext<TContext>(configureDb)
            .AddScoped<FleetContextBase, TContext>()
            .AddScoped<IFleetDbContext, TContext>();

        return builder;

    }
    public static FleetServiceBuilder AddPgContext(this FleetServiceBuilder builder, string connectionString)
    {
        return builder
             .AddDbContext<FleetPostgresContext>(db =>
             {
                 db
                     .UseNpgsql(connectionString, o =>
                     {
                         o
                             .MigrationsAssembly(typeof(FleetPostgresContext).Assembly.GetName().Name)
                             .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                     })
                     //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
#if DEBUG
                     .EnableDetailedErrors()
                     .EnableSensitiveDataLogging()
#endif
                     ;
             });
    }
    public static FleetServiceBuilder AddMySqlContext(this FleetServiceBuilder builder, string connectionString)
    {
        return builder.AddDbContext<FleetMySqlContext>(db =>
            {
                db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), o =>
                    {
                        o
                            .MigrationsAssembly(typeof(FleetMySqlContext).Assembly.GetName().Name)
                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    })
#if DEBUG
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
#endif
                    ;
            });
    }
    public static FleetServiceBuilder AddSqlServerContext(this FleetServiceBuilder builder, string connectionString)
    {
        return builder.AddDbContext<FleetSqlServerContext>(db =>
        {
            db.UseSqlServer(connectionString, o =>
            {
                o
                    .MigrationsAssembly(typeof(FleetSqlServerContext).Assembly.GetName().Name)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
#if DEBUG
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
#endif
                ;
        });
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
            .UseEntities<FleetContextBase>(c => c.ProfileAssemblies.Add(typeof(FleetProfile).Assembly));

        // Entity Items
        builder.Entities
           // Country
           .For<Country, string, CountryRepository>(e => e.AddMapping<CountryDto, CountryDto>())
           .For<Intervention, InterventionRepository, InterventionSearchObject, InterventionSortBy, InterventionIncludes>(e =>
           {
               e.HasRepository<InterventionRepository>();
               e.HasAttachments<FleetContextBase, Intervention, InterventionAttachment>();
           })
           .For<Brand, BrandRepository, BrandSearchObject, EntitySortBy, EntityIncludes>()
           .For<Vehicle, VehicleRepository, VehicleSearchObject, EntitySortBy, VehicleIncludes>(e =>
           {
               e.HasRepository<VehicleRepository>();
               e.HasAttachments<FleetContextBase, Vehicle, VehicleAttachment>();
           })
           .For<VehicleType, VehicleTypeRepository, VehicleTypeSearchObject, EntitySortBy, EntityIncludes>()
           .For<InterventionType, InterventionTypeRepository, InterventionTypeSearchObject, EntitySortBy, EntityIncludes>()
           .For<Operator, OperatorRepository, OperatorSearchObject, EntitySortBy, OperatorIncludes>(e =>
           {
               e.HasRepository<OperatorRepository>();
               e.HasAttachments<FleetContextBase, Operator, OperatorAttachment>();
           });

        builder
           // Normalizers
           .AddNormalizers(o =>
           {
               o
                   // helpers
                   .AddTransient<AddressNormalizer>()
                   .AddTransient(p => new PhoneNumberFormatter(p.GetRequiredService<ICultureContext>().Culture))
                   .AddTransient<ContactDataNormalizer>()
                   .AddTransient<EntityLabelNormalizer>()
                   .AddTransient<IdentificationNumberNormalizer>()
                   // simple normalizers
                   .AddTransient<IFleetEntityNormalizer<Brand>, FleetEntityNormalizer<Brand>>()
                   .AddTransient<IFleetEntityNormalizer<InterventionType>, FleetEntityNormalizer<InterventionType>>()
                   .AddTransient<IFleetEntityNormalizer<VehicleType>, FleetEntityNormalizer<VehicleType>>()
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
            .RegisterPrimerContainer<FleetContextBase>();

        return builder;
    }
}