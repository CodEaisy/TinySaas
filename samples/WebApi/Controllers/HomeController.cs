using CodEaisy.TinySaas.Interfaces;
using CodEaisy.TinySaas.Samples.WebApi.Authorization;
using CodEaisy.TinySaas.Samples.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly AppSingleton _appSingleton;
        private readonly TenantSingleton _tenantSingleton;
        private readonly AppScoped _appScoped;
        private readonly ITenantAccessor<SimpleTenant> _tenantAccessor;

        public HomeController(AppSingleton appSingleton, TenantSingleton tenantSingleton,
            AppScoped appScoped, ITenantAccessor<SimpleTenant> tenantAccessor)
        {
            _appSingleton = appSingleton;
            _tenantSingleton = tenantSingleton;
            _appScoped = appScoped;
            _tenantAccessor = tenantAccessor;
        }

        [HttpGet(nameof(Tenant))]
        public ActionResult Tenant()
        {
            return Ok(_tenantSingleton.GetTestValue());
        }

        [HttpGet(nameof(App))]
        public ActionResult App()
        {
            return Ok(_appSingleton.GetValue());
        }

        [HttpGet(nameof(Scoped))]
        public ActionResult Scoped()
        {
            return Ok(_appScoped.GetValues());
        }

        [HttpGet(nameof(Accessor))]
        public ActionResult Accessor()
        {
            return Ok(_tenantAccessor.Tenant.Name);
        }

        [Authorize(Policy = Policies.SimpleAuth)]
        [HttpGet(nameof(Authenticated))]
        public ActionResult Authenticated()
        {
            return Ok(nameof(Authenticated));
        }
    }
}
