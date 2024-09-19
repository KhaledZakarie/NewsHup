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

        /*
        public IActionResult Business()
        {
            var catId = context.Categories.FirstOrDefault(c => c.CategoryName == "Business")?.CategoryId;
            var artical = context.Articles.Where(a => a.CatId == catId).ToList();
            return View("index", artical);
        }
         public IActionResult Entertainment()
        {
            var catId= context.Categories.FirstOrDefault(c=>c.CategoryName== "Entertainment")?.CategoryId;
            var artical = context.Articles.Where(a=>a.CatId==catId).ToList();
            return View("index",artical);
        }
          public IActionResult Health()
        {
            var catId = context.Categories.FirstOrDefault(c => c.CategoryName == "Health")?.CategoryId;
            var artical = context.Articles.Where(a => a.CatId == catId).ToList();
            return View("index", artical);
        }
         public IActionResult Science()
        {

            var catId = context.Categories.FirstOrDefault(c => c.CategoryName == "Science")?.CategoryId;
            var artical = context.Articles.Where(a => a.CatId == catId).ToList();
            return View("index", artical);
        } 
        public IActionResult Sports()
        {
            var catId = context.Categories.FirstOrDefault(c => c.CategoryName == "Sports")?.CategoryId;
            var artical = context.Articles.Where(a => a.CatId == catId).ToList();
            return View("index", artical);
        }
        public IActionResult Technology()
        {
            var catId = context.Categories.FirstOrDefault(c => c.CategoryName == "Technology")?.CategoryId;
            var artical = context.Articles.Where(a => a.CatId == catId).ToList();
            return View("index", artical);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
