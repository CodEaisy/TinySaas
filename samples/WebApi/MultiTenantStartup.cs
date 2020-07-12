using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodEaisy.TinySaas.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Samples.WebApi
{
    public class MultiTenantStartup : IMultiTenantStartup<Tenant>
    {
        public void ConfigureServices(Tenant tenant, ContainerBuilder container)
        {
            #region inbuilt DI pattern
            // // to register services, you can either use inbuilt DI pattern
            // // 1. create a service collection
            // var services = new ServiceCollection();

            // // 2. register services to your service collection
            // // tenant singleton, registers a single instance for each tenant
            // services.AddSingleton<TenantSingleton>();

            // // 3. add the services into the container
            // container.Populate(services);
            #endregion

            #region AutoFac pattern

            // or register the service on the container directly using AutoFac
            container.RegisterType<TenantSingleton>().SingleInstance();

            #endregion
        }
    }
}
