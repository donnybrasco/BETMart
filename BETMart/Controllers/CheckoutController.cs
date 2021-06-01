using Microsoft.AspNetCore.Mvc;

namespace BETMart.Controllers
{
    public class CheckoutController 
        : ControllerBase<CheckoutController>
    {
        public CheckoutController()
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
