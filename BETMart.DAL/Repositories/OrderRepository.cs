﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BETMart.DAL.Core;
using BETMart.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BETMart.DAL.Repositories
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }

        Task<List<Order>> GetListAsync();

        Task<Order> GetByIdAsync(int orderId);

        Task<int> InsertAsync(Order order);

        Task UpdateAsync(Order order);

        Task DeleteAsync(Order order);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly IRepositoryAsync<Order> _repository;

        public OrderRepository(IRepositoryAsync<Order> repository)
        {
            _repository = repository;
        }

        public IQueryable<Order> Orders => _repository.Entities;

        public async Task DeleteAsync(Order order)
        {
            await _repository.DeleteAsync(order);
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _repository.Entities.Where(p => p.OrderId == orderId).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Order order)
        {
            await _repository.AddAsync(order);
            return order.OrderId;
        }

        public async Task UpdateAsync(Order order)
        {
            await _repository.UpdateAsync(order);
        }
    }
}
