using CodEaisy.TinySaas.Resolvers;
using CodEaisy.TinySaas.Stores;
using CodEaisy.TinySaas.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CodEaisy.TinySaas.MissingTenants;
using CodEaisy.TinySaas.Samples.WebApi.Options;
using CodEaisy.TinySaas.Samples.WebApi.Services;
using CodEaisy.TinySaas.Authorization;

namespace CodEaisy.TinySaas.Samples.WebApi
{
    public class Startup
    {
        private const string _tenancyConfig = "TenancyConfig";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // required to use ConfigTenancyStore
            services.AddOptions<ConfigTenancyOptions<SimpleTenant>>()
                .Bind(Configuration.GetSection(_tenancyConfig));

            // add multitenancy support,
            // added Tenant Model, Tenant Store provider, and resolution strategy
            services.AddMultitenancy<SimpleTenant, ConfigTenantStore<SimpleTenant>, QueryResolutionStrategy>()
                .AddPerTenantAuthorization();

            // add app level option
            services.AddOptions<AppOption>()
                .Bind(Configuration.GetSection(AppOption.Key));

            // add global singleton service
            services.AddSingleton<AppSingleton>();
            services.AddScoped<AppScoped>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // enable multitenant support, with missing tenant handler and tenant container
            app.UseMultitenancy<SimpleTenant>()
                .UseMissingTenantHandler<RedirectMissingTenantHandler, RedirectMissingTenantOptions>(options => options.RedirectUrl = "https://github.com/mimam419");

            app.UsePerTenantAuthentication();

            app.UseRouting();

            app.UsePerTenantAuthorization<SampleAuthorizationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
