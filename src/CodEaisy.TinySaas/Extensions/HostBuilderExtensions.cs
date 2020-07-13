using System;
using Autofac;
using CodEaisy.TinySaas.Core;
using CodEaisy.TinySaas.Interface;
using Microsoft.Extensions.Hosting;

namespace CodEaisy.TinySaas.Extensions
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// add multitenant service factory
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <typeparam name="TMultiTenantStartup"></typeparam>
        /// <typeparam name="TTenant"></typeparam>
        public static IHostBuilder ConfigureMultiTenancy<TMultiTenantStartup, TTenant>(this IHostBuilder hostBuilder)
            where TMultiTenantStartup : class, IMultiTenantStartup<TTenant>, new()
            where TTenant : ITenant
        {
            var multitenantStartup = new TMultiTenantStartup();
            hostBuilder.ConfigureMultiTenancy<TTenant>(multitenantStartup.ConfigureServices);
            return hostBuilder;
        }

        /// <summary>
        /// add multitenant services via an action
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="tenantServicesConfiguration"></param>
        /// <typeparam name="TTenant"></typeparam>
        public static IHostBuilder ConfigureMultiTenancy<TTenant>(this IHostBuilder hostBuilder,
            Action<TTenant, ContainerBuilder> tenantServicesConfiguration)
            where TTenant : ITenant
        {
            hostBuilder.UseServiceProviderFactory(new MultiTenantServiceProviderFactory<TTenant>(tenantServicesConfiguration));

            return hostBuilder;
        }
    }
}
