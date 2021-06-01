using AutoMapper;
using BETMart.BLL.Models;

namespace BETMart.Infrastructure.Profiles
{
    public class OrderProfile
        : Profile
    {
        public OrderProfile()
        {
            CreateMap<DAL.Entities.Order, Order>().ReverseMap();
        }
    }
}