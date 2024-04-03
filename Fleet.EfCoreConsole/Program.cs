using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Regira.Fleet.Data;
using Regira.Fleet.Identity.Data;

// prevent date errors in Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var host = CreateHostBuilder(args)
    .Build();

var accountContext = host.Services.GetRequiredService<AccountsContext>();
var fleetContext = host.Services.GetRequiredService<FleetContext>();

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

    services.AddDbContext<AccountsContext>(db =>
    {
        db.UseNpgsql(config["ConnectionStrings:FleetAccounts"]);
    });
    services.AddDbContext<FleetContext>(db =>
    {
        db.UseNpgsql(config["ConnectionStrings:FleetData"]);
    });
}
#endregion