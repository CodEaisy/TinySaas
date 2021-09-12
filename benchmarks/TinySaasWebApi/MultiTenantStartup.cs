using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodEaisy.TinySaas.Interfaces;
using TinySaasWebApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TinySaasWebApi
{
    public class MultitenantStartup : IMultitenantStartup<SimpleTenant>
    {
        public void ConfigureServices(SimpleTenant tenant, ContainerBuilder container)
        {
            #region inbuilt DI pattern
            // to register services, you can either use inbuilt DI pattern
            // 1. create a service collection
            var services = new ServiceCollection();

            // 2. register services to your service collection
            // tenant singleton, registers a single instance for each tenant
            services.AddSingleton<TenantSingleton>();

            // 3. add the services into the container
            container.Populate(services);
            #endregion
        }
    }
}
