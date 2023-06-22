using Microsoft.AspNetCore.Mvc;
using Shared.Services;

namespace Shared.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : AppControllerBase
    {
        public AppController(AppSingleton appSingleton): base(appSingleton) { }
    }
}
