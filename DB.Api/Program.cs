using ABDDB.Api.RequestCoordinator;
using ABDDB.Api.Services;
using ABDDB.Api.Utils;
using ABDDB.Replication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Https;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(o =>
{
    o.ConfigureHttpsDefaults(o =>
        o.ClientCertificateMode = ClientCertificateMode.AllowCertificate);
});

var config = ConfigUtils.GetConfiguration(builder.Configuration, args);

builder.ApplyConfiguration(config);
builder.AddServices(config);

var app = builder.Build();

// Configuring HTTP Pipeline
app.ConfigurePipeline();

// Mapping endpoints
app.MapPost("/store", [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)] (string key, string val, [FromServices] IRequestCoordinator coordinator) =>
    coordinator.PutAsync(key, val))
.WithName("Put");

app.MapGet("/store", [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)] async (string key, [FromServices] IRequestCoordinator coordinator) =>
{
    var result = await coordinator.GetAsync(key);
    return result is not null ? Results.Ok(result) : Results.NotFound();
})
.WithName("Get");

app.MapGet("/config", [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)] ([FromServices] IConfigurationService configurationService) =>
{
    var configuration = configurationService.ClusterConfiguration;
    var result = new
    {
        Nodes = configuration.Nodes,
        CurrentNodeId = configuration.CurrentNode.Id,
        ReadQuorum = configuration.ReadQuorum,
        WriteQuorum = configuration.WriteQuorum
    };
    return Results.Ok(result);
})
.WithName("GetClusterConfiguration");

app.MapGet("/connect", async (string userName, string password, [FromServices] IAuthenticationService authenticationService) =>
{
    return Results.Ok(await authenticationService.SignIn(userName, password));
})
.WithName("Connect");

app.Run();