using Microsoft.AspNetCore.Mvc;

namespace NewsHup.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
