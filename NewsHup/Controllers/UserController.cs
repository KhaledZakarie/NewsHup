using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsHup.Models;
using NewsHup.Repository;
using System.Security.Claims;
using TestMVC.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace NewsHup.Controllers
{


    public class UserController : Controller
    {

        // private NewsContext context = new NewsContext();

        private readonly NewsContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IHostingEnvironment hosting;

        // Constructor to inject NewsContext via DI
        public UserController(NewsContext context,IUserRepository userRepository, IHostingEnvironment _hosting)
        {
            _context = context;
            _userRepository = userRepository;
            hosting = _hosting;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Claims()
        {
            var claims = User.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });

            // Return the claims as JSON to make it easy to see in the browser
            return Json(claims);
        }

        //public IActionResult Register()
        //{
        //    return View();
        //}


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Register(User user)
            {


            if (ModelState.ErrorCount > 0)
            {



                return View("Login", user);



            }

            else
            {
                try
                {
                    //_context.Users.Add(user);
                    //_context.SaveChanges();

                    if (user.FormFile != null)///img uploaded
                    {
                        string FileName = user.FormFile.FileName;
                        string uploadFolder = Path.Combine(hosting.WebRootPath, "userImg");
                        string FullPath = Path.Combine(uploadFolder, FileName);
                        user.FormFile.CopyTo(new FileStream(FullPath, FileMode.Create));
                        user.UserImage = FileName;
                    }
                    else //put defualt img if not upload img
                    {
                        user.UserImage = "profileDefualt.jpg";
                    }


                    user.Role = Enums.Role.Writer;

                    var roleName = Enum.GetName(typeof(NewsHup.Enums.Role), user.Role);
                    

                    _userRepository.Add(user);
                    _userRepository.SaveChanges();

                    ClaimsIdentity claimsIdentity = new(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaim(new Claim("Id", user.Id.ToString()));
                    claimsIdentity.AddClaim(new Claim("Name", user.Name.ToString()));
                    claimsIdentity.AddClaim(new Claim("Email", user.Email.ToString()));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                    //claimsIdentity.AddClaim(new Claim("Role", user.Role.ToString()));
                    ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);


                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                    return View("Login",user);

                }

               // return RedirectToAction("Index", "Home");
                return RedirectToAction("GoHome", "User");
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



                var roleName = Enum.GetName(typeof(NewsHup.Enums.Role), userFromDB.Role);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, roleName));







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
        public IActionResult portfolio()
        {
            return View();
        }



    }
}