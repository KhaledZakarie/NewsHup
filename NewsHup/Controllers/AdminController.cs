using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;  // Assuming NewsContext is in this namespace

namespace NewsHup.Controllers
{
    public class AdminController : Controller
    {
        private readonly NewsContext _context;

        // Inject the NewsContext using dependency injection
        public AdminController(NewsContext context)
        {
            _context = context;
        }

        // New Dashboard action
        public IActionResult Dashboard()
        {
            return View();
        }

        // You can add more admin-related actions here in the future
    }
}
