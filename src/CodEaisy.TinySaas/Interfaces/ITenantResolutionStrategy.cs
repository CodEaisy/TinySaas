using System.Threading.Tasks;

namespace CodEaisy.TinySaas.Interfaces
{
    /// <summary>
    /// instruct how tenants should be resolved
    /// </summary>
    public interface ITenantResolutionStrategy
    {
        /// <summary>
        /// get tenant identifier value from HttpContext
        /// </summary>
        /// <returns>tenant identifier</returns>
        ValueTask<string> GetTenantIdentifierAsync();
    }
}
