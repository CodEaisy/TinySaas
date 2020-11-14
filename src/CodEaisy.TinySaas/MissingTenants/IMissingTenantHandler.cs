using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.MissingTenants
{
    /// <summary>
    /// missing tenant handler
    /// </summary>
    public interface IMissingTenantHandler
    {
        /// <summary>
        /// handle missing tenant gracefully
        /// </summary>
        /// <param name="httpContext"></param>
        Task Invoke(HttpContext httpContext);
    }
}
