using System.Threading.Tasks;

namespace CodEaisy.TinySaas.Interfaces
{
    /// <summary>
    /// tenant accessor as a service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITenantService<T> where T: ITenant
    {
        /// <summary>
        /// get current tenant
        /// </summary>
        /// <returns>tenant</returns>
        Task<T> GetTenant();
    }
}
