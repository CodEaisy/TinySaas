using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodEaisy.TinySaas.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Internals
{
    internal class MultitenantServiceProviderFactory<TTenant> : IServiceProviderFactory<ContainerBuilder>
        where TTenant : ITenant
    {
        public Action<TTenant, ContainerBuilder> _tenantServicesConfiguration;

        public MultitenantServiceProviderFactory(Action<TTenant, ContainerBuilder> tenantServicesConfiguration)
        {
            _tenantServicesConfiguration = tenantServicesConfiguration;
        }

        /// <summary>
        /// Create a builder populated with global services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>application container builder</returns>
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
        /// <returns>service provider</returns>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            MultitenantContainer<TTenant> container = null;

            MultitenantContainer<TTenant> containerAccessor() => container;

            containerBuilder
                .RegisterInstance((Func<MultitenantContainer<TTenant>>) containerAccessor)
                .SingleInstance();

            container = new MultitenantContainer<TTenant>(containerBuilder.Build(), _tenantServicesConfiguration);

            return new AutofacServiceProvider(containerAccessor());
        }
    }
}
