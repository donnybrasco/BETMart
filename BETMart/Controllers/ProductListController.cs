using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BETMart.BLL.Services;

namespace BETMart.Controllers
{
    public class ProductListController 
        : ControllerBase<ProductListController>
    {
        #region Ctor

        private readonly IProductService _service;

        public ProductListController(IProductService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllProducts();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetPaged()
        {
            var draw = Request.Form["draw"];
            string start = Request.Form["start"];
            string length = Request.Form["length"];
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][name]"];
            var sortColumnDir = Request.Form["order[0][dir]"];
            var searchValue = Request.Form["search[value]"];


            //Paging Size (10,20,50,100)    
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            var products = await _service.GetProductsPaged(skip, pageSize);
            //Sorting    
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    products = products.OrderBy(sortColumn + " " + sortColumnDir);
            //}
            //Search    
            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    products = products.Where(m => m.Name == searchValue);
            //}

            //total number of rows count  
            int recordsTotal = products.Count();
            
            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = products });
        }
    }
}
