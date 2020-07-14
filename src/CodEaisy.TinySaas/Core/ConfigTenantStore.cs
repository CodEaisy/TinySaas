using System.Collections.Generic;
using System.Threading.Tasks;
using CodEaisy.TinySaas.Interface;
using CodEaisy.TinySaas.Model;
using Microsoft.Extensions.Options;

namespace CodEaisy.TinySaas.Core
{
    public class ConfigTenancyOptions<TTenant> where TTenant : ITenant
    {
        public TenantIdentifier Identifier { get; set; }
        public List<TTenant> Tenants { get; set; }
    }

    /// <summary>
    /// Depends on ConfigTenancyOption_T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigTenantStore<T> : ITenantStore<T> where T: ITenant
    {
        private readonly List<T> _allTenants;
        private readonly TenantIdentifier _identifier;

        public ConfigTenantStore(IOptions<ConfigTenancyOptions<T>> options)
        {
            _allTenants = options.Value.Tenants;
            _identifier = options.Value.Identifier;
        }

        public ValueTask<T> GetTenant(string identifier)
        {
            var tenant = _identifier == TenantIdentifier.Id
                ? _allTenants.Find(t => t.Id.ToString() == identifier)
                : _allTenants.Find(t => t.Host == identifier);

            return new ValueTask<T>(tenant);
        }

        public ValueTask<IEnumerable<T>> GetAllTenants()
        {
            return new ValueTask<IEnumerable<T>>(_allTenants);
        }
    }
}
