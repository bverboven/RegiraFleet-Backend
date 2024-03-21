using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Regira.CRM.Identity.Web.DependencyInjection;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Authorization;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.DependencyInjection;
using Regira.Fleet.Identity.Entities;
using Regira.IO.Storage.FileSystem;
using Regira.Security.Abstractions;
using Regira.Security.Encryption;
using Regira.Serializing.Abstractions;
using Regira.Serializing.Newtonsoft.Json;
using Regira.Web.Swagger.Security;
using Serilog;
using System.Text.Json.Serialization;
using JsonSerializer = Regira.Serializing.Newtonsoft.Json.JsonSerializer;

namespace Regira.Fleet.Admin.Api.Infrastructure;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
        return builder;
    }
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services
            .AddControllers(_ =>
            {
                //var routePrefix = "api";
                //if (!string.IsNullOrWhiteSpace(routePrefix))
                //{
                //    o.UseCentralRoutePrefix(new RouteAttribute(routePrefix));
                //}
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

        services.AddAutoMapper(typeof(IdentityProfile).Assembly);

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
            .AddClientAdmin(c =>
            {
                var dataDirectory = config["Data:Directory"];
                c.ConnectionString = config["ConnectionStrings:FleetAccounts"];
                var fsConfig = new BinaryFileService.FileServiceOptions
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
            .AddDbContext<AccountsContext>(db => db.UseNpgsql(config["ConnectionStrings:FleetAccounts"], o => o.MigrationsAssembly(typeof(AccountsContext).Assembly.GetName().Name)))
            .AddFleetIdentity(o =>
            {
                var options = config.GetSection("Identity").Get<FleetIdentityOptions>()!;
                o.SecretKey = options.SecretKey;
                o.Audiences.AddRange(options.Audiences);
            });

        services.AddSingleton<IAuthorizationHandler, SuperUserRequirementHandler>();

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
