using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Regira.Fleet.DependencyInjection;
using Regira.Fleet.Identity.DependencyInjection;
using Regira.Fleet.Identity.Web.DependencyInjection;
using Regira.Fleet.Identity.Web.Filters;
using Regira.Fleet.Identity.Web.Middleware;
using Regira.Fleet.Statistics;
using Regira.IO.Storage.FileSystem;
using Regira.Office.Excel.Abstractions;
using Regira.Office.Mail.SendGrid;
using Regira.Security.Abstractions;
using Regira.Security.Encryption;
using Regira.Serializing.Abstractions;
using Regira.Serializing.Newtonsoft.Json;
using Regira.Web.Swagger.Security;
using Serilog;
using JsonSerializer = Regira.Serializing.Newtonsoft.Json.JsonSerializer;

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
            .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddNewtonsoftJson(o =>
            {
                //o.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                o.UseCamelCasing(true);
                var settings = o.SerializerSettings;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Include;
                var converters = settings.Converters;
                converters.Add(new StringEnumConverter());
                converters.Add(new BoolNumberConverter());
                converters.Add(new DateOnlyJsonConverter());
                converters.Add(new DateAndTimeConverter());
            });

        // global error handling
        //services.AddGlobalExceptionHandling();

        services.AddTransient<ISerializer, JsonSerializer>();

        services
            // Api routing
            .AddEndpointsApiExplorer()
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
            // Swagger (with auth)
            .AddSwaggerGen(c =>
            {
                c.AddJwtAuthentication();
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
            });

        services
            .AddScoped<StatisticsService>()
            .AddTransient<IExcelManager, FleetExcelManager>();

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
                    var key = config["SendGrid:Key"];
                    ArgumentException.ThrowIfNullOrWhiteSpace(key, "SendGrid API key");
                    return new SendGridMailer(new SendGridConfig { Key = key });
                });
            });

        return services;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

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