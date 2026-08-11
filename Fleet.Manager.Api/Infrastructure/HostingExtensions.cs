using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Regira.Entities.Web.Attachments.DependencyInjection;
using Regira.Fleet.DependencyInjection;
using Regira.Fleet.Identity.DependencyInjection;
using Regira.Fleet.Identity.Web.DependencyInjection;
using Regira.Fleet.Identity.Web.Filters;
using Regira.Fleet.Identity.Web.Middleware;
using Regira.Fleet.Statistics;
using Regira.IO.Storage.FileSystem;
using Regira.Licensing.DependencyInjection;
using Regira.Office.Excel.Abstractions;
using Regira.Office.Mail.MailGun;
using Regira.Security.Abstractions;
using Regira.Security.Authentication.Web.OpenApi.Transformers;
using Regira.Security.Encryption;
using Scalar.AspNetCore;
using Serilog;
using System.Text.Json.Serialization;

namespace Regira.Fleet.Manager.Api.Infrastructure;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
        );
        return builder;
    }
    public static IServiceCollection AddApi(this IServiceCollection services)
    {

        services
            .AddControllers(o =>
            {
                // Global filters for authorization
                o.Filters.Add<CanReadAuthorizationFilter>();
                o.Filters.Add<CanWriteAuthorizationFilter>();
            })
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                o.JsonSerializerOptions.AllowOutOfOrderMetadataProperties = true;
            });

        // global error handling
        //services.AddGlobalExceptionHandling();

        services
            // Enable Cors
            .AddCors(options =>
                options
                    .AddDefaultPolicy(policy => policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                    //.AllowCredentials()
                    //.WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
                    )
            )
            // OpenAPI (with JWT bearer security scheme)
            .AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            })
            // necessary services
            .AddHttpContextAccessor()
            .AddTransient<IEncrypter, SymmetricEncrypter>();

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddFleet(c =>
            {
                var dataDirectory = config["Data:Directory"];
                c.DatabaseType = config["Database:Fleet:Type"]!;
                c.ConnectionString = config["Database:Fleet:ConnectionString"]!;
                c.ConfigureStorageService(_ =>
                {
                    var fsConfig = new FileSystemOptions
                    {
                        RootFolder = dataDirectory!
                    };
                    return new BinaryFileService(fsConfig);
                });
                // Populate Uri on attachment DTOs — the SPA downloads saved attachments through it.
                // Without this the null resolver stays in place and every Uri comes back null.
                c.ConfigureEntities(e => e.UseAttachmentUris());
            });

        services
            .AddScoped<StatisticsService>()
            .AddTransient<IExcelService, FleetExcelManager>();

        return services;
    }
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddAccountsDbContext(config["Database:Accounts:ConnectionString"]!, config["Database:Accounts:Type"]!)
            .AddFleetIdentity(o =>
            {
                var options = config.GetSection("Identity").Get<FleetIdentityOptions>()!;
                o.SecretKey = options.SecretKey;
                o.Audiences.AddRange(options.Audiences);
                o.AddMailer(_ =>
                {
                    var mailConfig = new MailgunConfig
                    {
                        Api = config["MailGun:Api"] ?? throw new NullReferenceException("Config missing for MailGun:Api"),
                        Domain = config["MailGun:Domain"] ?? throw new NullReferenceException("Config missing for MailGun:Domain"),
                        Key = config["MailGun:Key"] ?? throw new NullReferenceException("Config missing for MailGun:Key")
                    };
                    return new MailGunMailer(mailConfig);
                });
            });

        return services;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        // OpenAPI + Scalar UI
        app.MapOpenApi()
            .AllowAnonymous();
        app.MapScalarApiReference(options =>
        {
            options.Authentication = new ScalarAuthenticationOptions
            {
                PreferredSecuritySchemes = [JwtBearerDefaults.AuthenticationScheme]
            };
        }).AllowAnonymous();

        app.UseHttpsRedirection();

        // order is important!
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAppContextLoader();

        // global exception handling
        //app.UseGlobalExceptionHandling();

        app
            .MapControllers()
            .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
            ;

        return app;
    }
}