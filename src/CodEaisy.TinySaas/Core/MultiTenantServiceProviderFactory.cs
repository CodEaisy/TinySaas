using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodEaisy.TinySaas.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Core
{
    public class MultiTenantServiceProviderFactory<TTenant> : IServiceProviderFactory<ContainerBuilder>
        where TTenant : ITenant
    {
        public Action<TTenant, ContainerBuilder> _tenantServicesConfiguration;

        public MultiTenantServiceProviderFactory(Action<TTenant, ContainerBuilder> tenantServicesConfiguration)
        {
            _tenantServicesConfiguration = tenantServicesConfiguration;
        }

        /// <summary>
        /// Create a builder populated with global services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            return builder;
        }

        /// <summary>
        /// Create our service provider
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            MultiTenantContainer<TTenant> container = null;

            MultiTenantContainer<TTenant> containerAccessor()
            {
                return container;
            }

            containerBuilder
                .RegisterInstance((Func<MultiTenantContainer<TTenant>>)containerAccessor)
                .SingleInstance();

            container = new MultiTenantContainer<TTenant>(containerBuilder.Build(), _tenantServicesConfiguration);

            return new AutofacServiceProvider(containerAccessor());
        }
    }
}
