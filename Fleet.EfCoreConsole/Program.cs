using Fleet.EfCoreConsole;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Regira.Fleet.Data;
using Regira.Fleet.DependencyInjection;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.DependencyInjection;
using Regira.IO.Storage.FileSystem;
using Regira.Security.Abstractions;
using Regira.Security.Encryption;

// prevent date errors in Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var host = CreateHostBuilder(args)
    .Build();

var accountContext = host.Services.GetRequiredService<AccountsContext>();
await accountContext.Database.EnsureDeletedAsync();
await accountContext.Database.EnsureCreatedAsync();

var accountSeeder = host.Services.GetRequiredService<AccountSeeder>();
var clients = await accountSeeder.Seed();

var fleetContext = host.Services.GetRequiredService<FleetContext>();
await fleetContext.Database.EnsureDeletedAsync();
await fleetContext.Database.EnsureCreatedAsync();

var dataSeeder = host.Services.GetRequiredService<DataSeeder>();
await dataSeeder.Seed(clients);

foreach (var sql in StatisticsViews.All)
{
    await fleetContext.Database.ExecuteSqlRawAsync(sql);
}

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

    services.AddAuthentication();
    services.AddFleetAuthentication();

    services.AddClientAdmin(c =>
    {
        var dataDirectory = config["Data:Directory"];
        c.ConnectionString = config["ConnectionStrings:FleetAccounts"];
        var fsConfig = new BinaryFileService.FileServiceOptions
        {
            RootFolder = dataDirectory!
        };
        c.ConfigureStorageService(_ => new BinaryFileService(fsConfig));
    });
    services.AddFleet(c =>
    {
        var dataDirectory = config["Data:Directory"];
        c.ConnectionString = config["ConnectionStrings:FleetData"];
        var fsConfig = new BinaryFileService.FileServiceOptions
        {
            RootFolder = dataDirectory!
        };
        c.ConfigureStorageService(_ => new BinaryFileService(fsConfig));
    });
}
#endregion