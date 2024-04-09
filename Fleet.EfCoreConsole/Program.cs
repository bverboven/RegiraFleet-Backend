using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Regira.Fleet.Data.MySQL;
using Regira.Fleet.Data.PostgreSQL;
using Regira.Fleet.Data.SqlServer;
using Regira.Fleet.Identity.Data.MySQL;
using Regira.Fleet.Identity.Data.PostgreSQL;
using Regira.Fleet.Identity.Data.SqlServer;

// prevent date errors in Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var host = CreateHostBuilder(args)
    .Build();

// PostgreSQL
var accountPgContext = host.Services.GetService<AccountsPostgresContext>();
var fleetPgContext = host.Services.GetService<FleetPostgresContext>();
// MySQL
var accountMySqlContext = host.Services.GetService<AccountsMySqlContext>();
var fleetMySqlContext = host.Services.GetService<FleetMySqlContext>();
// SqlServer
var accountSqlServerContext = host.Services.GetService<AccountsSqlServerContext>();
var fleetSqlServerContext = host.Services.GetService<FleetSqlServerContext>();

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
    var accountsConnectionString = config["Database:Accounts:ConnectionString"];
    var fleetConnectionString = config["Database:Fleet:ConnectionString"];

    // PostgreSQL
    //services.AddDbContext<AccountsPostgresContext>(db => db.UseNpgsql(accountsConnectionString, o => o.MigrationsAssembly(typeof(AccountsPostgresContext).Assembly.GetName().Name)));
    //services.AddDbContext<FleetPostgresContext>(db => db.UseNpgsql(fleetConnectionString, o => o.MigrationsAssembly(typeof(FleetPostgresContext).Assembly.GetName().Name)));
    // MySQL
    //services.AddDbContext<AccountsMySqlContext>(db => db.UseMySql(accountsConnectionString, ServerVersion.AutoDetect(accountsConnectionString), o => o.MigrationsAssembly(typeof(AccountsMySqlContext).Assembly.GetName().Name)));
    //services.AddDbContext<FleetMySqlContext>(db => db.UseMySql(fleetConnectionString, ServerVersion.AutoDetect(fleetConnectionString), o => o.MigrationsAssembly(typeof(FleetMySqlContext).Assembly.GetName().Name)));
    // SqlServer
    services.AddDbContext<AccountsSqlServerContext>(db => db.UseSqlServer(accountsConnectionString, o => o.MigrationsAssembly(typeof(AccountsSqlServerContext).Assembly.GetName().Name)));
    services.AddDbContext<FleetSqlServerContext>(db => db.UseSqlServer(fleetConnectionString, o => o.MigrationsAssembly(typeof(FleetSqlServerContext).Assembly.GetName().Name)));
}
#endregion