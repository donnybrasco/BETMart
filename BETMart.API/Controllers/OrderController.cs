using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BETMart.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BETMart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController
        : ControllerBase<OrderController>
    {
        private readonly IMapper _mapper;
        private readonly DAL.Repositories.IOrderRepository _repository;

        public OrderController(IMapper mapper, DAL.Repositories.IOrderRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<BLL.Models.Order>> Get()
        {
            try
            {
                var data = await _repository.GetListAsync();
                return data.Select(x => _mapper.Map<BLL.Models.Order>(x));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<BLL.Models.Order> Get(int orderId)
        {
            try
            {
                var data = await _repository.GetByIdAsync(orderId);
                return _mapper.Map<BLL.Models.Order>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<int> Insert(BLL.Models.Order order)
        {
            try
            {
                return await _repository.InsertAsync(_mapper.Map<Order>(order));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPut]
        public async Task Update(BLL.Models.Order order)
        {
            try
            {
                await _repository.UpdateAsync(_mapper.Map<Order>(order));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}