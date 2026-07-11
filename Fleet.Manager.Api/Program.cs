using Regira.Fleet.Manager.Api.Infrastructure;
using Regira.Licensing.DependencyInjection;
using Serilog;
using System.Reflection;

// prevent date errors in Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information($"Starting {Assembly.GetExecutingAssembly().GetName().Name}");

try
{
    var builder = WebApplication.CreateBuilder(args)
        .ConfigureSerilog();

    // services
    builder.Services
        .AddApi()
        .UseRegira(builder.Configuration)
        .AddServices(builder.Configuration)
        .AddIdentity(builder.Configuration);

    var app = builder.Build();

    app.ConfigureApp();

    app.Run();
}
catch (Exception ex)
{
    // Host error, logger might not be instanciated
    Log.Error(ex, "Host failed");
}
finally
{
    Log.CloseAndFlush();
}
