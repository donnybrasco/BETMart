using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BETMart.DAL.Core;
using BETMart.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BETMart.DAL.Repositories
{

    public interface IOrderDetailRepository
    {
        IQueryable<OrderDetail> OrderDetails { get; }

        Task<List<OrderDetail>> GetListAsync();

        Task<OrderDetail> GetByIdAsync(int orderDetailId);

        Task<int> InsertAsync(OrderDetail orderDetail);

        Task UpdateAsync(OrderDetail orderDetail);

        Task DeleteAsync(OrderDetail orderDetail);
    }
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly IRepositoryAsync<OrderDetail> _repository;

        public OrderDetailRepository(IRepositoryAsync<OrderDetail> repository)
        {
            _repository = repository;
        }

        public IQueryable<OrderDetail> OrderDetails => _repository.Entities;

        public async Task DeleteAsync(OrderDetail orderDetail)
        {
            await _repository.DeleteAsync(orderDetail);
        }

        public async Task<OrderDetail> GetByIdAsync(int orderDetailId)
        {
            return await _repository.Entities.Where(p => p.OrderDetailId == orderDetailId).FirstOrDefaultAsync();
        }

        public async Task<List<OrderDetail>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(OrderDetail orderDetail)
        {
            await _repository.AddAsync(orderDetail);
            return orderDetail.OrderDetailId;
        }

        public async Task UpdateAsync(OrderDetail orderDetail)
        {
            await _repository.UpdateAsync(orderDetail);
        }
    }
}
