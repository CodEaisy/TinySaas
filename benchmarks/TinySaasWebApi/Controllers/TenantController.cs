using Shared.Controllers;
using Shared.Services;

namespace TinySaasWebApi.Controllers
{
    public class TenantController : TenantControllerBase
    {
        public TenantController(TenantSingleton tenantSingleton): base(tenantSingleton)
         { }
    }
}
