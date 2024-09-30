using Microsoft.AspNetCore.Mvc;
using NewsHup.Models;
using System.Diagnostics;
using TestMVC.Models;

namespace NewsHup.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public NewsContext context = new NewsContext();
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string area="")
        {
            List<Article> artical;
            if (string.IsNullOrEmpty(area)) {
                artical = context.Articles.ToList();
                
            }
            else
            {
                var catId = context.Categories.FirstOrDefault(c => c.CategoryName == area)?.CategoryId;
                 artical = context.Articles.Where(a => a.CatId == catId).ToList();

            }


            return View(artical);

        }
        // New Dashboard action
        public IActionResult Dashboard()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
