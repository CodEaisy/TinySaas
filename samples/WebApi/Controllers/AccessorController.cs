using CodEaisy.TinySaas.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessorController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index() => Ok(HttpContext.GetCurrentTenant<SimpleTenant>().Name);

        [HttpGet("full")]
        public ActionResult Full() => Ok(HttpContext.GetCurrentTenant<SimpleTenant>());
    }
}
