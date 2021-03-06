using AutoMapper;
using BETMart.BLL.Models;

namespace BETMart.Infrastructure.Profiles
{
    public class CustomerProfile
        : Profile
    {
        public CustomerProfile()
        {
            CreateMap<DAL.Entities.User, Customer>().ReverseMap();
            CreateMap<DAL.Entities.Address, Customer>().ReverseMap();
        }
    }
}