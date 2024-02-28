using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Entities.Models;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.Cars;
using Regira.Fleet.Entities.Cars.Brands;
using Regira.Fleet.Entities.Cars.CarTypes;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Suppliers;
using Regira.Fleet.Entities.Suppliers.SupplierTypes;
using Regira.Fleet.Statistics;
using Regira.Office.Excel.Abstractions;
using Regira.Security.Abstractions;
using Regira.Security.Encryption;
using Regira.Serializing.Abstractions;
using Regira.Serializing.Newtonsoft.Json;
using Regira.Web.Swagger.Security;
using JsonSerializer = Regira.Serializing.Newtonsoft.Json.JsonSerializer;

namespace Regira.Fleet.Api.Infrastructure;

public static class HostingExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration config)
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
    public static IServiceCollection AddFleet(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddAutoMapper(typeof(FleetProfile).Assembly)
            .AddDbContext<FleetContext>(db =>
            {
                var connectionString = config["ConnectionStrings:AcaFleetNet"];
                db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

        services
            .AddScoped<StatisticsService>()
            .AddTransient<IExcelManager, AcaExcelManager>();

        services
            .UseEntities<FleetContext>(c => c.ProfileAssemblies.Add(typeof(FleetProfile).Assembly))
            .For<Intervention, InterventionRepository, InterventionSearchObject, EntitySortBy, InterventionIncludes>(e => e.AddMapping<Intervention, InterventionInputDto>())
            .For<Brand, BrandRepository, BrandSearchObject, EntitySortBy, EntityIncludes>(e => e.AddMapping<Brand, BrandInputDto>())
            .For<Car, CarRepository, CarSearchObject, EntitySortBy, EntityIncludes>(e => e.AddMapping<Car, CarInputDto>())
            .For<CarType, CarTypeRepository, CarTypeSearchObject, EntitySortBy, EntityIncludes>(e => e.AddMapping<CarType, CarTypeInputDto>())
            .For<InterventionType, InterventionTypeRepository, InterventionTypeSearchObject, EntitySortBy, EntityIncludes>(e => e.AddMapping<InterventionType, InterventionTypeInputDto>())
            .For<Supplier, SupplierRepository, SupplierSearchObject, EntitySortBy, EntityIncludes>(e => e.AddMapping<Supplier, SupplierInputDto>())
            .For<SupplierType, SupplierTypeRepository, SupplierTypeSearchObject, EntitySortBy, EntityIncludes>(e => e.AddMapping<SupplierType, SupplierTypeInputDto>());

        return services;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        // order is important!
        app.UseCors();


        // Authorization header renamed to X-Authorization header for compatibility with Swagger (ignores the normal Authorization header)
        // cf. https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1295#issuecomment-588297906
        app.Use((httpContext, next) => // For the oauth2-less!
        {
            if (httpContext.Request.Headers.TryGetValue("X-Authorization", out var authHeader))
            {
                httpContext.Request.Headers.Append("Authorization", authHeader);
            }

            return next();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        // global exception handling
        //app.UseGlobalExceptionHandling();

        app
            .MapControllers()
            .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
            ;

        return app;
    }
}