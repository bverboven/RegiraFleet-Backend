using Regira.Fleet.Api.Infrastructure;

// prevent date errors in Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// services
builder.Services
    .AddApi()
    .AddServices(builder.Configuration)
    .AddIdentity(builder.Configuration)
    ;

var app = builder.Build();

app.ConfigureApp();

app.Run();
