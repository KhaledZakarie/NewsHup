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


        // Fetch Users from the database and pass them to the view
        public IActionResult Users()
        {
            var users = _context.Users.ToList(); // Fetch all users from the DB
            return View(users);  // Pass the list of users to the view
        }
    }
}
