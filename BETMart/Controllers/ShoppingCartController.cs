using System.Threading.Tasks;
using BETMart.BLL._Core;
using BETMart.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BETMart.Controllers
{
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
            var response = await _service.GetShoppingCart();
            var model = new ShoppingCartViewModel
            {
                Current = response.Data,
                ErrorMessage = response.Message
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToShoppingCart(int productId)
        {
            var response = await _service.AddToShoppingCart(productId, 1);
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
