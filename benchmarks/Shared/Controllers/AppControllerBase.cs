using Microsoft.AspNetCore.Mvc;
using Shared.Services;

namespace Shared.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppControllerBase : ControllerBase
    {
        private readonly AppSingleton _appSingleton;

        public AppControllerBase(AppSingleton appSingleton) => _appSingleton = appSingleton;

        [HttpGet]
        public ActionResult Index() => Ok(_appSingleton.GetValue());
    }
}
