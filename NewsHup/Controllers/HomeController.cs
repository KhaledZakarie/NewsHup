using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsHup.Models;
using System.Diagnostics;
using TestMVC.Models;

namespace NewsHup.Controllers
{

    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //  public NewsContext context = new NewsContext();
        private readonly NewsContext _context;

        // Use Dependency Injection to inject NewsContext
        public HomeController(ILogger<HomeController> logger, NewsContext context)
        {
            _logger = logger;
            _context = context;
        }




        public IActionResult Index(string area="")
        {
            List<Article> artical;
            if (string.IsNullOrEmpty(area)) {
                artical = _context.Articles.ToList();
                
            }
            else
            {
                var catId = _context.Categories.FirstOrDefault(c => c.CategoryName == area)?.CategoryId;
                 artical = _context.Articles.Where(a => a.CatId == catId).ToList();

            }


            return View(artical);

        }


        public IActionResult GetPartial(string area = "")
        {
            List<Article> articals;
            if (string.IsNullOrEmpty(area))
            {
                articals = _context.Articles.ToList();

            }
            else
            {
                var catId = _context.Categories.FirstOrDefault(c => c.CategoryName == area)?.CategoryId;
                articals = _context.Articles.Where(a => a.CatId == catId).ToList();

            }


            return PartialView("_HomePartial", articals);

        }


        // Action method for error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
