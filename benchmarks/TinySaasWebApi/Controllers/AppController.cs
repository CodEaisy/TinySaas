using TinySaasWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace TinySaasWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
        private readonly AppSingleton _appSingleton;

        public AppController(AppSingleton appSingleton) => _appSingleton = appSingleton;

        [HttpGet]
        public ActionResult Index() => Ok(_appSingleton.GetValue());
    }
}
