using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsHup.Models;
using TestMVC.Models;

namespace NewsHup.Controllers
{
    public class UserController : Controller
    {

        // private NewsContext context = new NewsContext();

        private readonly NewsContext _context;

        // Constructor to inject NewsContext via DI
        public UserController(NewsContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Register(User user)
        {


            if (ModelState.ErrorCount > 0)
            {


              
                return View(user);



            }

            else
            {
                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                    return View(user);

                }

                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Login(User user)
        {
            // Check if the model state is valid (e.g., all required fields are provided and valid)
            if (!ModelState.IsValid)
            {
                return View(user);  // If the model state is invalid, return the view with the validation messages
            }

            // Check if the user exists in the database with the provided email and password
            var userFromDB = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

            // If the user is not found (email and password do not match)
            if (userFromDB == null)
            {
                // Add an error to the ModelState to display on the form
                ModelState.AddModelError("Password", "Invalid email or password.");
                return View(user);  // Re-render the view with the error message
            }

            // If login is successful, redirect to the home page (or wherever you want to go)
            return RedirectToAction("Index", "Home");
        }


    }
}
