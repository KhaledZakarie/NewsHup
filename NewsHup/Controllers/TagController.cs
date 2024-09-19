using Microsoft.AspNetCore.Mvc;

namespace NewsHup.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
