using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Entities.EFcore.Attachments;
using Regira.Fleet.Clients;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Core.DependencyInjection;
using Regira.Fleet.Core.GlobalQueryFilters;
using Regira.Fleet.Core.Models;
using Regira.Fleet.Data;
using Regira.Fleet.Data.MySQL;
using Regira.Fleet.Data.PostgreSQL;
using Regira.Fleet.Data.SqlServer;
using Regira.Fleet.Entities.Countries;
using Regira.Fleet.Entities.EntityLabels;
using Regira.Fleet.Entities.InterventionOperators;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Models;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.Vehicles;
using Regira.IO.Storage.Abstractions;
using Regira.Normalizing;
using Regira.Normalizing.Models;
using FilterHasNormalizedContentQueryBuilder = Regira.Entities.EFcore.QueryBuilders.GlobalFilterBuilders.FilterHasNormalizedContentQueryBuilder;
using PgFilterHasNormalizedContentQueryBuilder = Regira.Fleet.Data.PostgreSQL.QueryBuilders.FilterHasNormalizedContentQueryBuilder;


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
            DataBaseTypes.PostgreSQL => builder.AddPgContext(builder.Options.ConnectionString),
            DataBaseTypes.MySQL => builder.AddMySqlContext(builder.Options.ConnectionString),
            DataBaseTypes.SqlServer => builder.AddSqlServerContext(builder.Options.ConnectionString),
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
            .UseEntities<FleetContextBase>(c =>
            {
                c.ProfileAssemblies.Add(typeof(FleetProfile).Assembly);
                c.AddDefaultQKeywordHelper(_ => new DefaultNormalizer(
                    new NormalizeOptions
                    {
                        RemoveDiacritics = true,
                        Transform = TextTransform.ToUpperCase
                    }
                ));
                c.AddDefaultPrimers();
                c.AddDefaultEntityNormalizer();
                c.AddNormalizer<EntityLabelNormalizer, IEntityLabel>();
                c.AddDefaultGlobalQueryFilters();
                // make sure only allowed clientId items are loaded
                c.AddGlobalFilterQueryBuilder<FilterHasClientQueryBuilder>();
                if (options.DatabaseType == DataBaseTypes.PostgreSQL)
                {
                    c.AddGlobalFilterQueryBuilder<PgFilterHasNormalizedContentQueryBuilder>();
                }
                else
                {
                    c.AddGlobalFilterQueryBuilder<FilterHasNormalizedContentQueryBuilder>();
                }
            });

        // Entity Items
        builder.Entities
           .AddCountries()
           .AddInterventions()
           .AddVehicles()
           .AddOperators();

        return builder;
    }
    public static FleetServiceBuilder AddAttachmentServices(this FleetServiceBuilder builder, Func<IServiceProvider, IFileService> configure)
    {
        builder.Entities
            .ConfigureAttachmentService(configure)
            .ConfigureTypedAttachmentService(db => (
            [
                db.InterventionAttachments.ToDescriptor<Intervention>(),
                db.InterventionOperatorAttachments.ToDescriptor<Operator>(),
                db.VehicleAttachments.ToDescriptor<Vehicle>()
            ]));

        return builder;
    }
}