using CodEaisy.TinySaas.Core.Resolvers;
using CodEaisy.TinySaas.Core.Stores;
using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.Middlewares;
using CodEaisy.TinySaas.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.Configure<ConfigTenancyOptions<Tenant>>(Configuration.GetSection(_tenancyConfig));

            // add multitenancy support,
            // added Tenant Model, Tenant Store provider, and resolution strategy
            services.AddMultiTenancy<Tenant, ConfigTenantStore<Tenant>, QueryResolutionStrategy>();

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
            app.UseMultitenancy<Tenant, RedirectMissingTenantHandler, RedirectMissingTenantOptions>(new RedirectMissingTenantOptions());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
