using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodEaisy.TinySaas.Interface;
using Microsoft.Extensions.Options;

namespace CodEaisy.TinySaas.Core.Stores
{
    public class ConfigTenancyOptions<TTenant> where TTenant : ITenant
    {
        public List<TTenant> Tenants { get; set; }
    }

    /// <summary>
    /// Depends on ConfigTenancyOption_T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigTenantStore<T> : ITenantStore<T> where T: class, ITenant
    {
        private readonly ConcurrentDictionary<string, T> _allTenants;

        public ConfigTenantStore(IOptions<ConfigTenancyOptions<T>> options)
        {
            _allTenants = new ConcurrentDictionary<string, T>(options.Value.Tenants
                .ToDictionary(x => x.Identifier, x => x));
        }

        public ValueTask<T> GetTenant(string identifier)
        {
            T tenant = default;

            if(!string.IsNullOrEmpty(identifier))
                _allTenants.TryGetValue(identifier, out tenant);

            return new ValueTask<T>(tenant);
        }

        public ValueTask<IEnumerable<T>> GetAllTenants()
        {
            return new ValueTask<IEnumerable<T>>(_allTenants.Values);
        }
    }
}
