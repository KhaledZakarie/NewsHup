using Microsoft.AspNetCore.Mvc;

namespace NewsHup.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied"); 
        }

    }
}
