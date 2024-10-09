using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsHup.Models;
using NewsHup.Repository;
using System.Security.Claims;
using TestMVC.Models;

namespace NewsHup.Controllers
{


    public class UserController : Controller
    {

        // private NewsContext context = new NewsContext();

        private readonly NewsContext _context;
        private readonly IUserRepository _userRepository;

        // Constructor to inject NewsContext via DI
        public UserController(NewsContext context,IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
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
                    //_context.Users.Add(user);
                    //_context.SaveChanges();


                    _userRepository.Add(user);
                    _userRepository.SaveChanges();

                    ClaimsIdentity claimsIdentity = new(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaim(new Claim("Id", user.Id.ToString()));
                    claimsIdentity.AddClaim(new Claim("Name", user.Name.ToString()));
                    claimsIdentity.AddClaim(new Claim("Email", user.Email.ToString()));
                    claimsIdentity.AddClaim(new Claim("Role", user.Role.ToString()));
                    ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);


                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                    return View(user);

                }

               // return RedirectToAction("Index", "Home");
                return RedirectToAction("AuthGo", "User");
            }
        }



        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
               
                return View();
            }

            return RedirectToAction("GoHome");

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
                //var userFromDB = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
                var userFromDB = _userRepository.GetUserBy(u => u.Email == user.Email && u.Password == user.Password);
                if (userFromDB == null)
                {
                    ModelState.AddModelError("Password", "Wrong Password !");
                    return View(user);
                }

                ClaimsIdentity claimsIdentity = new(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim("Id", userFromDB.Id.ToString()));
                claimsIdentity.AddClaim(new Claim("Name", userFromDB.Name.ToString()));
                claimsIdentity.AddClaim(new Claim("Email", userFromDB.Email.ToString()));
                claimsIdentity.AddClaim(new Claim("Role", userFromDB.Role.ToString()));
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                //return RedirectToAction("Index", "Home");




                return RedirectToAction("GoHome");
            }
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult GoHome()
        {
            return RedirectToAction("Index", "Home");
        }



    }
}