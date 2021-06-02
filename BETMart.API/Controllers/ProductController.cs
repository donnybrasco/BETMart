using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BETMart.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BETMart.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IEnumerable<BLL.Models.Product>> Get()
        {
            try
            {
                var data = await _repository.GetListAsync();
                return data.Select(x => _mapper.Map<BLL.Models.Product>(x));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<BLL.Models.Product> Get(int productId)
        {
            try
            {
                var data = await _repository.GetByIdAsync(productId);
                return _mapper.Map<BLL.Models.Product>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<int> Insert(BLL.Models.Product product)
        {
            try
            {
                return await _repository.InsertAsync(_mapper.Map<Product>(product));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPut]
        public async Task Update(BLL.Models.Product product)
        {
            try
            {
                await _repository.UpdateAsync(_mapper.Map<Product>(product));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpDelete]
        public async Task Delete(int productId)
        {
            try
            {
                var data = await _repository.GetByIdAsync(productId);
                await _repository.DeleteAsync(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
