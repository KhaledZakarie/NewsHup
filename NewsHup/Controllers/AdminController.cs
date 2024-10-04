using Microsoft.AspNetCore.Mvc;
using NewsHup.Models;
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
        // This will handle the POST request to add a new user
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                // Add the user to the database
                _context.Users.Add(user);
                _context.SaveChanges();

                return Json(new { success = true, message = "User added successfully!" });
            }

            // Collect all validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            // Return the errors in the response
            // If the data is invalid, return an error response
            return Json(new { success = false, message = "Invalid data!", errors });
        }


        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            // Find the user by ID
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                // Remove the user from the database
                _context.Users.Remove(user);
                _context.SaveChanges();
                return Json(new { success = true, message = "User deleted successfully!" });
            }
            return Json(new { success = false, message = "User not found!" });
        }

    }
}
