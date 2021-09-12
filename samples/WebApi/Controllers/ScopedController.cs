using CodEaisy.TinySaas.Samples.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScopedController : ControllerBase
    {
        private readonly AppScoped _appSingleton;

        public ScopedController(AppScoped appSingleton) => _appSingleton = appSingleton;

        [HttpGet]
        public ActionResult Index() => Ok(_appSingleton.GetValues());
    }
}
