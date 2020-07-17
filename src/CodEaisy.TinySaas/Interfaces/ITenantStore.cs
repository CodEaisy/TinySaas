using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodEaisy.TinySaas.Interfaces
{
    /// <summary>
    /// tenant store, feel-free to implement it anyway you like
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITenantStore<T> where T : ITenant
    {
        /// <summary>
        /// get tenant by identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>tenant with provided identifier</returns>
        ValueTask<T> GetTenant(string identifier);

        /// <summary>
        /// returns all available tenants
        /// </summary>
        /// <returns>list of tenants</returns>
        ValueTask<IEnumerable<T>> GetAllTenants();
    }
}
