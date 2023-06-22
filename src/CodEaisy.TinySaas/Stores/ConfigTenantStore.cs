using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CodEaisy.TinySaas.Interfaces;
using Microsoft.Extensions.Options;

namespace CodEaisy.TinySaas.Stores
{
    /// <summary>
    /// use configuration file as tenant store
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class ConfigTenancyOptions<TTenant> where TTenant : ITenant
    {
        /// <summary>
        /// list of available tenants
        /// </summary>
        public List<TTenant> Tenants { get; set; }
    }

    /// <summary>
    /// Depends on ConfigTenancyOption_T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigTenantStore<T> : ITenantStore<T> where T: class, ITenant
    {
        private readonly ReadOnlyDictionary<string, T> _allTenants;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public ConfigTenantStore(IOptions<ConfigTenancyOptions<T>> options)
        {
            _allTenants = new ReadOnlyDictionary<string, T>(options.Value.Tenants
                .ToDictionary(x => x.Identifier, x => x));
        }

        /// <summary>
        /// get tenant by identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>tenant</returns>
        public ValueTask<T> GetTenant(string identifier)
        {
            T tenant = default;

            if(!string.IsNullOrEmpty(identifier))
                _allTenants.TryGetValue(identifier, out tenant);

            return new ValueTask<T>(tenant);
        }

        /// <summary>
        /// get an enumerable containing all tenants
        /// </summary>
        /// <returns>all tenant</returns>
        public ValueTask<IEnumerable<T>> GetAllTenants()
        {
            return new ValueTask<IEnumerable<T>>(_allTenants.Values);
        }
    }
}
