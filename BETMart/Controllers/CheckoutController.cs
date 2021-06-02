using System.Threading.Tasks;
using BETMart.BLL._Core;
using BETMart.BLL.Services;
using BETMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BETMart.Controllers
{
    [Authorize]
    public class CheckoutController 
        : ControllerBase<CheckoutController>
    {
        #region Ctor

        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public CheckoutController(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        #endregion
        public async Task<IActionResult> Index()
        {
            var response = await _orderService.GetCurrentOrder();
            var model = new CheckoutViewModel
            {
                Name = _userService.Name,
                Current = response.Data
            };
            return View(model);
        }
    }
}
