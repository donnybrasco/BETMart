using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BETMart.BLL.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BETMart.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            using HttpClient client = new HttpClient { BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
            string stringData = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(stringData, Encoding.UTF8, Common.Common.ContentType.Json);
            HttpResponseMessage response = await client.PostAsync(Common.Common.APIEndpoint.Login, contentData);
            await response.Content.ReadAsStringAsync();

            return RedirectToAction("", "Product");
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            throw new NotImplementedException();
            return RedirectToAction("", "Product");
        }
    }
}
