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

            
            if (ModelState.ErrorCount > 1)
            {


                return View(user);



            }

            else
            {
               var userFromDB =  _context.Users.FirstOrDefault(u=>  u.Email == user.Email && u.Password == user.Password );
                if (userFromDB ==null )
                {
                    ModelState.AddModelError("Password", "Wrong Password !");
                    return View(user);

                }


                return RedirectToAction("Index", "Home");
            }
        }

    }
}
