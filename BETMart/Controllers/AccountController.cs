using Microsoft.AspNetCore.Mvc;

namespace BETMart.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View("Login");
        }
        public IActionResult Register()
        {
            return View("Register");
        }
    }
}
