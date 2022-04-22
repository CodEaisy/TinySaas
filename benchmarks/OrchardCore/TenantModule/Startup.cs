using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;

namespace TenantModule
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TenantSingleton>();
        }

        public void Configure(IEndpointRouteBuilder endpoints)
        {

        }
    }
}