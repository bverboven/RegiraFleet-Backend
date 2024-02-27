using Regira.Fleet.Api.Infrastructure;
using Regira.Fleet.Authentication;

var builder = WebApplication.CreateBuilder(args);

// services
builder.Services
    .AddApi(builder.Configuration)
    .AddFleet(builder.Configuration)
    .AddFleetAuthentication(builder.Configuration);

var app = builder.Build();

app.ConfigureApp();

app.Run();
