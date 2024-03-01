using Fleet.EfCoreConsole;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Regira.Fleet.Data;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.DependencyInjection;
using Regira.Security.Abstractions;
using Regira.Security.Encryption;

// prevent date errors in Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var host = CreateHostBuilder(args)
    .Build();

var accountContext = host.Services.GetRequiredService<AccountsContext>();
await accountContext.Database.EnsureDeletedAsync();
await accountContext.Database.EnsureCreatedAsync();
var fleetContext = host.Services.GetRequiredService<FleetContext>();
await fleetContext.Database.EnsureDeletedAsync();
await fleetContext.Database.EnsureCreatedAsync();

var dataSeeder = host.Services.GetRequiredService<DataSeeder>();
var clients = await dataSeeder.Seed();
var accountSeeder = host.Services.GetRequiredService<AccountSeeder>();
await accountSeeder.Seed(clients);

Console.WriteLine("Created Host");

// used by EF Core to access the DbContext
#region Startup
static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(SetupConfig)
        .ConfigureServices(ConfigureServices);
}
static void SetupConfig(HostBuilderContext context, IConfigurationBuilder builder)
{
    builder.Sources.Clear();
    // add configuration
    builder
        .AddJsonFile("appsettings.json", true, true)
        .AddUserSecrets(typeof(Program).Assembly, true);
}
static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    var config = context.Configuration;

    services
        .AddTransient<IEncrypter, SymmetricEncrypter>()
        .AddTransient<DataSeeder>()
        .AddTransient<AccountSeeder>();

    // PostgreSQL
    services
        // IdentityContext
        .AddDbContext<AccountsContext>(db => db.UseNpgsql(config["ConnectionStrings:FleetAccounts"], o => o.MigrationsAssembly(typeof(AccountsContext).Assembly.GetName().Name)));
    services
        // Fleet Data
        .AddDbContext<FleetContext>(db => db.UseNpgsql(config["ConnectionStrings:FleetData"], o => o.MigrationsAssembly(typeof(FleetContext).Assembly.GetName().Name)));

    services.AddAuthentication();
    services.AddFleetAuthentication();
}
#endregion