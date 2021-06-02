using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BETMart.BLL.Services;
using Microsoft.AspNetCore.Authorization;

namespace BETMart.Controllers
{
    [Authorize]
    public class ProductController 
        : ControllerBase<ProductController>
    {
        #region Ctor

        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllProducts();
            return View(data);
        }

        public async Task<IActionResult> View(int productId)
        {
            if (productId == default(int)) throw new ArgumentNullException($"Product Id is null");
            var data = await _service.GetProduct(productId);
            return View(data);
        }
    }
}
