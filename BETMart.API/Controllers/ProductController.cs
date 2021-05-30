using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BETMart.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController 
        : ControllerBase<ProductController>
    {
        private readonly IMapper _mapper;
        private readonly DAL.Repositories.IProductRepository _repository;

        public ProductController(IMapper mapper, DAL.Repositories.IProductRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<BLL.Models.Product> Get()
        {
            return _repository.Products.Select(x => _mapper.Map<BLL.Models.Product>(x));
        }
    }
}
