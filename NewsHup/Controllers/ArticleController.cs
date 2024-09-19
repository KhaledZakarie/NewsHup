using Microsoft.AspNetCore.Mvc;

namespace NewsHup.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
