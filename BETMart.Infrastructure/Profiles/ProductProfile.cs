using AutoMapper;
using BETMart.BLL.Models;

namespace BETMart.Infrastructure.Profiles
{
    public class ProductProfile
        : Profile
    {
        public ProductProfile()
        {
            CreateMap<DAL.Entities.Product, Product>().ReverseMap();
        }
    }
}
