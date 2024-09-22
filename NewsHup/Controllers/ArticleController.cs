using Microsoft.AspNetCore.Mvc;

namespace NewsHup.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddArticle()
        {
            return View();
        }
        public IActionResult MyArticle()
        {
            return View();
        }
    }
}
