using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Interface
{
    public interface IMissingTenantHandler
    {
        Task Invoke(HttpContext httpContext);
    }
}
