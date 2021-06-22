using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BaltaStore.Api.Controllers
{
    public class HomeController : Controller
    {
        
        [HttpGet]
        [Route("")]
        
        public object Get()
        {
            return new { version = "Version 0.0.1" };
        }

    }
}
