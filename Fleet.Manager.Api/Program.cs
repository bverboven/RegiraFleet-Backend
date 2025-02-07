using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Manager.Api.Infrastructure;
using Regira.Fleet.Models.Vehicles.Brands;

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
