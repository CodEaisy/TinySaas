using System;
using CodEaisy.TinySaas.Interface;
using CodEaisy.TinySaas.Model;
using Microsoft.Extensions.Options;

namespace CodEaisy.TinySaas.Core.Internals
{
    /// <summary>
    /// Tenant aware options cache
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TTenant"></typeparam>
    internal class TenantOptionsCache<TOptions, TTenant> : IOptionsMonitorCache<TOptions>
        where TOptions : class
        where TTenant : ITenant
    {
        private readonly ITenantAccessor<TTenant> _tenantAccessor;
        private readonly TenantOptionsCacheDictionary<TOptions> _tenantSpecificOptionsCache =
            new TenantOptionsCacheDictionary<TOptions>();

        public TenantOptionsCache(ITenantAccessor<TTenant> tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        public void Clear()
        {
            _tenantSpecificOptionsCache.Get(_tenantAccessor.Tenant.Identifier).Clear();
        }

        public TOptions GetOrAdd(string name, Func<TOptions> createOptions)
        {
            return _tenantSpecificOptionsCache.Get(_tenantAccessor.Tenant.Identifier)
                .GetOrAdd(name, createOptions);
        }

        public bool TryAdd(string name, TOptions options)
        {
            return _tenantSpecificOptionsCache.Get(_tenantAccessor.Tenant.Identifier)
                .TryAdd(name, options);
        }

        public bool TryRemove(string name)
        {
            return _tenantSpecificOptionsCache.Get(_tenantAccessor.Tenant.Identifier)
                .TryRemove(name);
        }
    }
}
