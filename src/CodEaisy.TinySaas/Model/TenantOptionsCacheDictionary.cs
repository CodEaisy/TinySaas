using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace CodEaisy.TinySaas.Model
{
    /// <summary>
    /// Dictionary of tenant specific options caches
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    internal class TenantOptionsCacheDictionary<TOptions> where TOptions : class
    {
        /// <summary>
        /// Caches stored in memory
        /// </summary>
        private readonly ConcurrentDictionary<Guid, IOptionsMonitorCache<TOptions>> _tenantSpecificOptionCaches =
            new ConcurrentDictionary<Guid, IOptionsMonitorCache<TOptions>>();

        /// <summary>
        /// Get options for specific tenant (create if not exists)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public IOptionsMonitorCache<TOptions> Get(Guid tenantId)
        {
            return _tenantSpecificOptionCaches.GetOrAdd(tenantId, new OptionsCache<TOptions>());
        }
    }
}
