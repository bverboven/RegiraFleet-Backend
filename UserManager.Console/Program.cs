using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.DependencyInjection;
using Regira.Fleet.Identity.Models.Users;
using Regira.IO.Storage.FileSystem;
using Regira.Security.Abstractions;
using Regira.Security.Encryption;
using UserManager.Console;

// prevent date errors in Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var host = CreateHostBuilder(args)
    .Build();

//var accountContext = host.Services.GetRequiredService<AccountsContextBase>();

var config = host.Services.GetRequiredService<IConfiguration>();
var userManager = host.Services.GetRequiredService<UserManager<FleetUser>>();
var roleManager = host.Services.GetRequiredService<RoleManager<IdentityRole>>();

var superUserExists = await roleManager.RoleExistsAsync(FleetClaimTypes.SuperUser);
if (!superUserExists)
{
    await roleManager.CreateAsync(new IdentityRole(FleetClaimTypes.SuperUser));
}

var adminUsers = config.GetSection("Identity:AdminUsers").Get<AdminUser[]>()!;
foreach (var item in adminUsers)
{
    var user = await userManager.FindByEmailAsync(item.Email);
    if (user == null)
    {
        user = new FleetUser { Email = item.Email, UserName = item.Email, Culture = item.Culture };
        var response = await userManager.CreateAsync(user, item.Password);
        Console.WriteLine($"Creating User {item.Email}: {response.Succeeded}");
    }
    var roles = await userManager.GetRolesAsync(user);
    if (!roles.Contains(FleetClaimTypes.SuperUser))
    {
        await userManager.AddToRoleAsync(user, FleetClaimTypes.SuperUser);
    }
}

Console.WriteLine("The End");

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
        .AddTransient<IEncrypter, SymmetricEncrypter>();

    services.AddAuthentication();
    services.AddFleetAuthentication();

    services.AddIdentityWithAdmin(c =>
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
}
#endregion