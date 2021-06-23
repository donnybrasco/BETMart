using System.Threading.Tasks;
using BETMart.BLL.Models;
using BETMart.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BETMart.Controllers
{
    [Authorize]
    public class ShoppingCartController 
        : ControllerBase<ShoppingCartController>
    {
        #region Ctor
        
        private readonly IOrderService _service;

        public ShoppingCartController(IOrderService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            var response = await _service.GetCurrentOrder();
            var model = new ShoppingCartViewModel
            {
                Current = response.Data ?? new Order(),
                ErrorMessage = response.Message
            };
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToShoppingCart(int productId)
        {
            var response = await _service.AddToShoppingCart(productId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShoppingCart(int orderDetailId, int quantity)
        {
            var response = await _service.UpdateShoppingCart(orderDetailId, quantity);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromShoppingCart(int orderDetailId)
        {
            var response = await _service.RemoveFromShoppingCart(orderDetailId);
            return Ok(response);
        }
    }
}
