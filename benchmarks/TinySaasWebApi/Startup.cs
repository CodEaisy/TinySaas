using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CodEaisy.TinySaas;
using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.MissingTenants;
using CodEaisy.TinySaas.Resolvers;
using CodEaisy.TinySaas.Stores;
using Shared.Services;

namespace TinySaasWebApi
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
            services.AddMultitenancy<SimpleTenant, ConfigTenantStore<SimpleTenant>, QueryResolutionStrategy>();

            // add global singleton service
            services.AddSingleton<AppSingleton>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // enable multitenant support, with missing tenant handler and tenant container
            app.UseMultitenancy<SimpleTenant>()
                .UseMissingTenantHandler<RedirectMissingTenantHandler, RedirectMissingTenantOptions>(options => options.RedirectUrl = "https://github.com/mimam419");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
