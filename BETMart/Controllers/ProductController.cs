using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BETMart.BLL.Services;

namespace BETMart.Controllers
{
    public class ProductController : Controller
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
    }
}
