using Microsoft.AspNetCore.Mvc;
using Shared.Controllers;
using Shared.Services;

namespace OrchardCoreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenantController : TenantControllerBase
    {
        public TenantController(TenantSingleton tenantSingleton): base(tenantSingleton)
         { }
    }
}
