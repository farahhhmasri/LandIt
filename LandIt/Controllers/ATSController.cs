using Microsoft.AspNetCore.Mvc;

namespace LandIt.Controllers
{
    public class ATSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
