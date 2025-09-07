using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.DAL.EFcore.Services;
using Regira.Entities.DependencyInjection.Mapping;
using Regira.Entities.DependencyInjection.QueryBuilders;
using Regira.Entities.DependencyInjection.ServiceBuilders.Extensions;
using Regira.Entities.EFcore.Normalizing;
using Regira.Entities.EFcore.Primers;
using Regira.Entities.EFcore.QueryBuilders.GlobalFilterBuilders;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Core.DependencyInjection;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Data.MySQL;
using Regira.Fleet.Identity.Data.PostgreSQL;
using Regira.Fleet.Identity.Data.SqlServer;
using Regira.Fleet.Identity.DependencyInjection.Entities;
using Regira.Fleet.Identity.Models;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Services;
using Regira.IO.Storage.Abstractions;
using Regira.Normalizing.Models;
using PgFilterHasNormalizedContentQueryBuilder = Regira.Fleet.Identity.Data.PostgreSQL.QueryBuilders.FilterHasNormalizedContentQueryBuilder;

namespace Regira.Fleet.Identity.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static FleetServiceBuilder AddIdentityWithAdmin(this IServiceCollection services, Action<FleetHostingOptions> configure)
    {
        var options = new FleetHostingOptions();
        configure.Invoke(options);

        services
            // Database context
            .AddAccountsDbContext(options.ConnectionString, options.DatabaseType);

        var builder = new FleetServiceBuilder(services, options);

        // Entities
        builder.AddFleetEntities(options);

        // Attachments
        builder.AddAttachmentServices(options.FileServiceFactory ?? throw new InvalidOperationException($"No implementation for {nameof(IFileService)} configured"));

        return builder;
    }

    public static IServiceCollection AddAccountsDbContext(this IServiceCollection services, string connectionString, string type)
    {
        return type switch
        {
            DataBaseTypes.PostgreSQL => services.AddPgContext(connectionString),
            DataBaseTypes.MySQL => services.AddMySqlContext(connectionString),
            DataBaseTypes.SqlServer => services.AddSqlServerContext(connectionString),
            _ => throw new NotSupportedException($"Type {type} not supported"),
        };
    }
    public static IServiceCollection AddAccountsDbContext<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDb)
        where TContext : AccountsContextBase
    {
        return services
            .AddDbContext<TContext>((sp, db) =>
            {
                configureDb(db);
                db.AddPrimerInterceptors(sp);
                db.AddNormalizerInterceptors(sp);
                db.AddAutoTruncateInterceptors();
            })
            .AddScoped<AccountsContextBase, TContext>()
            .AddScoped<IAccountsDbContext, TContext>();
    }
    public static IServiceCollection AddMySqlContext(this IServiceCollection services, string connectionString)
    {
        return services.AddAccountsDbContext<AccountsMySqlContext>(db =>
            {
                db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), o =>
                {
                    o
                        .MigrationsAssembly(typeof(AccountsMySqlContext).Assembly)
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
#if DEBUG
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
#endif
                    ;
            });
    }
    public static IServiceCollection AddPgContext(this IServiceCollection services, string connectionString)
    {
        return services.AddAccountsDbContext<AccountsPostgresContext>(db =>
        {
            db
                .UseNpgsql(connectionString, o =>
                {
                    o
                        .MigrationsAssembly(typeof(AccountsPostgresContext).Assembly)
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
    public static IServiceCollection AddSqlServerContext(this IServiceCollection services, string connectionString)
    {
        return services.AddAccountsDbContext<AccountsSqlServerContext>(db =>
        {
            db
                .UseSqlServer(connectionString, o =>
                {
                    o
                        .MigrationsAssembly(typeof(AccountsSqlServerContext).Assembly)
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

    public static FleetServiceBuilder AddFleetEntities(this FleetServiceBuilder builder, FleetHostingOptions options)
    {
        builder.Services
             // Entity context
             .UseEntities<AccountsContextBase>(c =>
             {
                 c.UseAutoMapper([typeof(IdentityProfile).Assembly]);
                 c.UseDefaults(ed => ed.ConfigureNormalizing(o => o.Transform = TextTransform.ToUpperCase));

                 c.AddGlobalFilterQueryBuilder<FilterIdsQueryBuilder<string>>();
                 if (options.DatabaseType == DataBaseTypes.PostgreSQL)
                 {
                     c.AddGlobalFilterQueryBuilder<PgFilterHasNormalizedContentQueryBuilder>();
                 }
                 else
                 {
                     c.AddGlobalFilterQueryBuilder<FilterHasNormalizedContentQueryBuilder>();
                 }
             });


        builder.Entities
            // Entity context
            .AddTenants()
            .AddFleetUsers(options.DatabaseType);

        return builder;
    }
    public static IServiceCollection AddTenantClaims(this IServiceCollection services)
    {
        services
              .AddHttpContextAccessor()
              .AddTransient<ITenantUserClaimsService, IdentityTenantUserClaimsService>()
              ;

        return services;
    }
    public static FleetServiceBuilder AddAttachmentServices(this FleetServiceBuilder builder, Func<IServiceProvider, IFileService> configure)
    {
        builder.Entities
            .WithAttachments(configure)
            .ConfigureTypedAttachmentService(_ => (
            [
            ]));

        return builder;
    }

    public static IdentityBuilder AddFleetAuthentication(this IServiceCollection services, FleetAuthenticationOptions options)
    {
        var builder = services
            .AddIdentityCore<FleetUser>(o =>
            {
                o.SignIn.RequireConfirmedAccount = false;
                o.User.RequireUniqueEmail = true;
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredUniqueChars = 1;
                o.Password.RequiredLength = 2;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                o.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AccountsContextBase>()
            .AddUserManager<FleetUserIdentityManager>()
            .AddClaimsPrincipalFactory<FleetUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        // Tenant UserClaims
        services.AddTenantClaims();

        if (options.MailerFactory != null)
        {
            services.AddTransient(options.MailerFactory);
            services.AddTransient<IEmailSender, IdentityMailer>();
        }

        return builder;
    }
    public static IdentityBuilder AddFleetAuthentication(this IServiceCollection services, Action<FleetAuthenticationOptions>? configure = null)
    {
        var options = new FleetAuthenticationOptions();
        configure?.Invoke(options);

        return AddFleetAuthentication(services, options);
    }
}