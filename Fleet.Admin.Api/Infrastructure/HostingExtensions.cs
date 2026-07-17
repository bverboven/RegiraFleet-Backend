using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Authorization;
using Regira.Fleet.Identity.DependencyInjection;
using Regira.Fleet.Identity.Web.DependencyInjection;
using Regira.IO.Storage.FileSystem;
using Regira.Office.Mail.MailGun;
using Regira.Security.Abstractions;
using Regira.Security.Authentication.Web.OpenApi.Transformers;
using Regira.Security.Encryption;
using Scalar.AspNetCore;
using Serilog;
using System.Text.Json.Serialization;

namespace Regira.Fleet.Admin.Api.Infrastructure;

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
            .AddControllers()
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
            .AddIdentityWithAdmin(c =>
            {
                var dataDirectory = config["Data:Directory"];
                c.DatabaseType = config["Database:Accounts:Type"]!;
                c.ConnectionString = config["Database:Accounts:ConnectionString"]!;
                var fsConfig = new FileSystemOptions
                {
                    RootFolder = dataDirectory!
                };
                c.ConfigureStorageService(_ => new BinaryFileService(fsConfig));
            });

        return services;
    }
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
    {
        services
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

        // Requirements
        services.AddSingleton<IAuthorizationHandler, SuperUserRequirementHandler>();

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

        // global exception handling
        //app.UseGlobalExceptionHandling();

        app
            .MapControllers()
            .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
            .RequireAuthorization(FleetPolicies.SuperUserPolicy)
            ;

        return app;
    }
}
