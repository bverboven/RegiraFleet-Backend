using Regira.Fleet.Admin.Api.Infrastructure;
using Serilog;

// prevent date errors in Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args)
    .ConfigureSerilog();

// services
builder.Services
    .AddApi()
    .AddServices(builder.Configuration)
    .AddIdentity(builder.Configuration);

var app = builder.Build();

app.ConfigureApp();

app.Run();
