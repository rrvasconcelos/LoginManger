using LoginManager.Configuration;
using LoginManager.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiConfiguration(builder.Configuration)
    .AddIdentityConfiguration(builder.Configuration)
    .AddDependencyInjectionConfiguration();

var app = builder.Build();

app.UseApiConfiguration(app.Environment);

app.MapEndpoints();

app.Run();