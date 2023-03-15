using CodEaisy.TinySaas;
using CodEaisy.TinySaas.Authorization;
using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.MissingTenants;
using CodEaisy.TinySaas.Resolvers;
using CodEaisy.TinySaas.Samples.WebApi;
using CodEaisy.TinySaas.Samples.WebApi.Options;
using CodEaisy.TinySaas.Samples.WebApi.Services;
using CodEaisy.TinySaas.Stores;

var builder = WebApplication.CreateBuilder(args);

// added multitenancy to Host builder
builder.Host.ConfigureMultitenancy<MultitenantStartup, SimpleTenant>();

// get configuration
var Configuration = builder.Configuration;
// Add services to the container.

// required to use ConfigTenancyStore
builder.Services.AddOptions<ConfigTenancyOptions<SimpleTenant>>()
    .Bind(Configuration.GetSection("TenancyConfig"));

// add multitenancy support,
// added Tenant Model, Tenant Store provider, and resolution strategy
builder.Services.AddMultitenancy<SimpleTenant, ConfigTenantStore<SimpleTenant>, QueryResolutionStrategy>()
    .AddPerTenantAuthorization();

// add app level option
builder.Services.AddOptions<AppOption>()
    .Bind(Configuration.GetSection(AppOption.Key));

// add global singleton service
builder.Services.AddSingleton<AppSingleton>();
builder.Services.AddScoped<AppScoped>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// enable multitenant support, with missing tenant handler and tenant container
app.UseMultitenancy<SimpleTenant>()
    .UseMissingTenantHandler<RedirectMissingTenantHandler, RedirectMissingTenantOptions>(options => options.RedirectUrl = "https://github.com/mimam419");

app.UsePerTenantAuthentication();

app.UseRouting();

app.UsePerTenantAuthorization<SampleAuthorizationMiddleware>();

app.MapControllers();

app.Run();
