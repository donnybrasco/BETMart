using AutoMapper;
using BETMart.BLL.Models;

namespace BETMart.Infrastructure.Profiles
{
    public class OrderDetailProfile
        : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<DAL.Entities.OrderDetail, OrderDetail>().ReverseMap();
        }
    }
}