using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.DAL.EFcore.Normalizing;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Entities.EFcore.Abstractions;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Services;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.DependencyInjection;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Core.Normalizing.Abstractions;
using Regira.Fleet.Core.Primers;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Entities;
using Regira.Fleet.Identity.Entities.Clients;
using Regira.Fleet.Identity.Entities.Clients.Subscriptions;
using Regira.Fleet.Identity.Entities.Users;
using Regira.Fleet.Identity.Services;
using Regira.IO.Storage.Abstractions;
using Regira.Normalizing;
using Regira.Normalizing.Abstractions;
using Regira.Normalizing.Models;
using Regira.Office.Mail.Abstractions;
using Regira.Security.Authentication.Mail;

namespace Regira.Fleet.Identity.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public class FleetAuthenticateOptions
    {
        internal Func<IServiceProvider, IMailer>? MailerFactory;
        public void AddMailer(Func<IServiceProvider, IMailer> factory)
        {
            MailerFactory = factory;
        }
    }

    public static FleetServiceBuilder<AccountsContext> AddClientAdmin(this IServiceCollection services, Action<FleetHostingOptions> configure)
    {
        var options = new FleetHostingOptions();
        configure.Invoke(options);

        var builder = new FleetServiceBuilder<AccountsContext>(services, options)
            // Database context
            .AddDbContext(options.ConnectionString!);

        builder.Services
            //Entity context
            .UseEntities<AccountsContext>(c => c.ProfileAssemblies.Add(typeof(IdentityProfile).Assembly));

        builder.Entities
           // Entity context
           .For<Client, string, ClientRepository, ClientSearchObject, EntitySortBy, ClientIncludes>(e =>
           {
               e.HasRepository<ClientRepository>();
           })
          .For<ClientSubscription, int, ClientSubscriptionSearchObject>(e =>
           {
           })
          .For<FleetUser, string, FleetUserRepository, FleetUserSearchObject, EntitySortBy, EntityIncludes>(e =>
          {
              e.HasRepository<FleetUserRepository>();
          });

        // Attachments
        builder
            .AddAttachmentServices(options.FileServiceFactory ?? throw new InvalidOperationException($"No implementation for {nameof(IFileService)} configured"));

        builder
            .AddNormalizers(o => o.AddTransient<IFleetEntityNormalizer<Client>, FleetEntityNormalizer<Client>>())
            .AddPrimers();

        return builder;
    }
    public static FleetServiceBuilder<AccountsContext> AddDbContext(this FleetServiceBuilder<AccountsContext> builder, string connectionString)
    {
        builder.Services
             //.AddDbContext<FleetContext>(db => db.UseMySql(options.ConnectionString, ServerVersion.AutoDetect(options.ConnectionString)));
             .AddDbContext<AccountsContext>(db =>
             {
                 db
                     .UseNpgsql(connectionString, o =>
                     {
                         o
                             .MigrationsAssembly(typeof(AccountsContext).Assembly.GetName().Name)
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
    public static IServiceCollection AddClientClaims(this IServiceCollection services)
    {
        services
              .AddHttpContextAccessor()
              .AddTransient<IClientUserClaimsService, IdentityClientUserClaimsService>()
              ;

        return services;
    }
    public static FleetServiceBuilder<AccountsContext> AddAttachmentServices(this FleetServiceBuilder<AccountsContext> builder, Func<IServiceProvider, IFileService> configure)
    {
        builder.Entities
            .ConfigureAttachmentService(configure)
            .ConfigureTypedAttachmentService(db => (new IAttachmentQuerySetDescriptor[]
            {
            }));

        return builder;
    }
    public static FleetServiceBuilder<AccountsContext> AddNormalizers(this FleetServiceBuilder<AccountsContext> builder, Action<IServiceCollection> configure)
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
    public static FleetServiceBuilder<AccountsContext> AddPrimers(this FleetServiceBuilder<AccountsContext> builder)
    {
        builder.Services
            .AddTransient<IEntityPrimer<IHasCreated>, HasCreatedDbPrimer>()
            .AddTransient<IEntityPrimer<IHasLastModified>, HasLastModifiedDbPrimer>()
            .RegisterPrimerContainer<AccountsContext>();

        return builder;
    }

    public static IdentityBuilder AddFleetAuthentication(this IServiceCollection services, FleetAuthenticateOptions options)
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
            .AddEntityFrameworkStores<AccountsContext>()
            .AddUserManager<FleetUserIdentityManager>()
            .AddClaimsPrincipalFactory<FleetUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        // Client UserClaims
        services.AddClientClaims();

        if (options.MailerFactory != null)
        {
            services.AddTransient(options.MailerFactory);
            services.AddTransient<IEmailSender, IdentityMailer>();
        }

        return builder;
    }
    public static IdentityBuilder AddFleetAuthentication(this IServiceCollection services, Action<FleetAuthenticateOptions>? configure = null)
    {
        var options = new FleetAuthenticateOptions();
        configure?.Invoke(options);

        return AddFleetAuthentication(services, options);
    }
}