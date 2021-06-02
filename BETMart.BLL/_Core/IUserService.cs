using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BETMart.BLL._Core
{
    public interface IUserService
    {
        string Name { get; }
        string UserId { get; }
        string Token { get; }
    }
    public class UserService
        : IUserService
    {
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) == null ? null : httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            Name = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name) == null ? null : httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value;
            Token = httpContextAccessor.HttpContext?.User?.FindFirst("access_token") == null ? null : httpContextAccessor.HttpContext?.User?.FindFirst("access_token").Value;
        }

        public string UserId { get; }
        public string Name { get; }
        public string Token { get; }
    }
}
