using Regira.Fleet.Api.Infrastructure;

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
