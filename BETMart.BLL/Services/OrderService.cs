﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using BETMart.BLL._Core;
using BETMart.Common;
using BETMart.DAL.Core;
using BETMart.DAL.Entities;
using BETMart.DAL.Repositories;
using Order = BETMart.BLL.Models.Order;

namespace BETMart.BLL.Services
{
    public interface IOrderService
    {
        Task<Response> AddToShoppingCart(int productId);
        Task<Response<Order>> GetCurrentOrder();
        Task<Response> UpdateShoppingCart(int orderDetailId, int quantity);
        Task<Response> RemoveFromShoppingCart(int orderDetailId);
    }
    public class OrderService
        : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        #region Ctor

        public OrderService(
            IOrderRepository orderRepository, 
            IOrderDetailRepository orderDetailRepository, 
            IProductRepository productRepository, 
            IUnitOfWork unitOfWork, 
            IUserService userService,
            IMapper mapper)  
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        #endregion

        public async Task<Response> AddToShoppingCart(int productId)
        {
            try
            {
                var order = await _orderRepository.FindFirstAsync(x => x.CustomerId == _userService.UserId);
                if (order == null)
                {
                    order = new DAL.Entities.Order
                    {
                        OrderDate = DateTime.Now,
                        OrderStatus = OrderStatus.New,
                        CustomerId = _userService.UserId
                    };
                    await _orderRepository.InsertAsync(order);
                }

                var product = await _productRepository.GetByIdAsync(productId);
                var orderDetail = new OrderDetail
                {
                    ProductId = productId,
                    Quantity = 1,
                    Order = order,
                    Price = product.Price
                };
                
                await _orderDetailRepository.InsertAsync(orderDetail);

                await _unitOfWork.Commit(_userService.Name);

                return Response<Order>.Success(_mapper.Map<Order>(order), $"{product.Name} add successfully!");
            }
            catch (Exception ex)
            {
                return Response.Fail(ex.Message);
            }
        }

        public async Task<Response<Order>> GetCurrentOrder()
        {
            try
            {
                var order = await _orderRepository.FindFirstAsync(x => x.CustomerId == _userService.UserId && x.OrderStatus == OrderStatus.New);
                if (order == null)
                {
                    return new Response<Order>
                    {
                        IsSuccessful = false,
                        Message = "No shopping cart exists for user"
                    };
                }
                if (order.OrderDetails.Count == default(int))
                {
                    return new Response<Order>
                    {
                        IsSuccessful = false,
                        Message = "No shopping items have been captured"
                    };
                }

                return new Response<Order>
                {
                    IsSuccessful = true,
                    Data = _mapper.Map<Order>(order)
                };
            }
            catch (Exception ex)
            {
                return Response<Order>.Fail(ex.Message);
            }
        }

        public async Task<Response> UpdateShoppingCart(int orderDetailId, int quantity)
        {
            try
            {
                var orderDetail = await _orderDetailRepository.GetByIdAsync(orderDetailId);
                orderDetail.Quantity = quantity;

                await _orderDetailRepository.UpdateAsync(orderDetail);

                await _unitOfWork.Commit(_userService.Name);

                return Response.Success("Update successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail(ex.Message);
            }
        }

        public async Task<Response> RemoveFromShoppingCart(int orderDetailId)
        {
            try
            {
                var orderDetail = await _orderDetailRepository.GetByIdAsync(orderDetailId);
                await _orderDetailRepository.DeleteAsync(orderDetail);

                await _unitOfWork.Commit();

                return Response.Success("Removed successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail(ex.Message);
            }
        }
    }
}
