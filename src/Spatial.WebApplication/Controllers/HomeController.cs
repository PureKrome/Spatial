using Microsoft.AspNetCore.Mvc;

namespace Spatial.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        
    }
}