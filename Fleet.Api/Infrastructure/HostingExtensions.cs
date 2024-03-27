using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Regira.Fleet.DependencyInjection;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Web.DependencyInjection;
using Regira.Fleet.Identity.Web.Filters;
using Regira.Fleet.Identity.Web.Middleware;
using Regira.Fleet.Statistics;
using Regira.IO.Storage.FileSystem;
using Regira.Office.Excel.Abstractions;
using Regira.Security.Abstractions;
using Regira.Security.Encryption;
using Regira.Serializing.Abstractions;
using Regira.Serializing.Newtonsoft.Json;
using Regira.Web.Swagger.Security;
using System.Text.Json.Serialization;
using JsonSerializer = Regira.Serializing.Newtonsoft.Json.JsonSerializer;

namespace Regira.Fleet.Api.Infrastructure;

public static class HostingExtensions
{
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
                c.ConnectionString = config["ConnectionStrings:FleetData"];
                var fsConfig = new BinaryFileService.FileServiceOptions
                {
                    RootFolder = dataDirectory!
                };
                c.ConfigureStorageService(_ => new BinaryFileService(fsConfig));
            });

        services
            .AddScoped<StatisticsService>()
            .AddTransient<IExcelManager, AcaExcelManager>();

        return services;
    }
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddDbContext<AccountsContext>(db => db.UseNpgsql(config["ConnectionStrings:FleetAccounts"], o => o.MigrationsAssembly(typeof(AccountsContext).Assembly.GetName().Name)))
            .AddFleetIdentity(o =>
            {
                var options = config.GetSection("Identity").Get<FleetIdentityOptions>()!;
                o.SecretKey = options.SecretKey;
                o.Audiences.AddRange(options.Audiences);
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