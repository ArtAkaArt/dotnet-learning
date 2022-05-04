using Microsoft.AspNetCore.Mvc;

namespace Sol3.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
