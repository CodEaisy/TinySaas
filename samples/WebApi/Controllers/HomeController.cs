using System;
using System.Threading.Tasks;
using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly TenantSingleton _singleton;
        private readonly ITenantService<Tenant> _tenantService;

        public HomeController(TenantSingleton singleton, ITenantService<Tenant> tenantService)
        {
            _singleton = singleton;
            _tenantService = tenantService;
        }

        [HttpGet("tenant")]
        public async Task<ActionResult> GetTenantName()
        {
            var tenantFromService = await _tenantService.GetTenant();
            var tenantFromContext = HttpContext.GetCurrentTenant<Tenant>();

            return Ok(new {
                NameFromContext = tenantFromContext.Identifier,
                NameFromService = tenantFromService.Identifier,
            });
        }

        [HttpGet("singleton")]
        public Guid TestTenantSingleton()
        {
            return _singleton.GetTestValue();
        }
    }
}
