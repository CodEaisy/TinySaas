using System;
using Autofac;
using CodEaisy.TinySaas.Internals;
using CodEaisy.TinySaas.Interfaces;
using Microsoft.Extensions.Hosting;

namespace CodEaisy.TinySaas.Extensions
{
    /// <summary>
    /// host builder extensions
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// add multitenant service factory
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <typeparam name="TMultitenantStartup"></typeparam>
        /// <typeparam name="TTenant"></typeparam>
        public static IHostBuilder ConfigureMultitenancy<TMultitenantStartup, TTenant>(this IHostBuilder hostBuilder)
            where TMultitenantStartup : class, IMultitenantStartup<TTenant>, new()
            where TTenant : class, ITenant
        {
            var multitenantStartup = new TMultitenantStartup();
            hostBuilder.ConfigureMultitenancy<TTenant>(multitenantStartup.ConfigureServices);
            return hostBuilder;
        }

        /// <summary>
        /// add multitenant services via an action
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="tenantServicesConfiguration"></param>
        /// <typeparam name="TTenant"></typeparam>
        public static IHostBuilder ConfigureMultitenancy<TTenant>(this IHostBuilder hostBuilder,
            Action<TTenant, ContainerBuilder> tenantServicesConfiguration)
            where TTenant : class, ITenant
        {
            hostBuilder.UseServiceProviderFactory(new MultitenantServiceProviderFactory<TTenant>(tenantServicesConfiguration));

            return hostBuilder;
        }
    }
}
