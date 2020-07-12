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

            hostBuilder.UseServiceProviderFactory(new MultiTenantServiceProviderFactory<TTenant>(multitenantStartup.ConfigureServices));

            return hostBuilder;
        }
    }
}
