using Microsoft.AspNetCore.Mvc;

namespace NewsHup.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
