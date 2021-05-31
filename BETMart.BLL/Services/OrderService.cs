using System;
using System.Threading.Tasks;
using BETMart.BLL._Core;
using BETMart.BLL.Models;

namespace BETMart.BLL.Services
{
    public interface IOrderService
    {
        Task<Response> AddToShoppingCart(Order order);
    }
    public class OrderService
        : IOrderService
    {
        #region Ctor

        public OrderService()
        {
            
        }

        #endregion

        public async Task<Response> AddToShoppingCart(Order order)
        {
            throw new NotImplementedException("");
        }
    }
}
