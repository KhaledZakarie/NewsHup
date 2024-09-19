using Microsoft.AspNetCore.Mvc;

namespace NewsHup.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
