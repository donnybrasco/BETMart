using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BETMart.BLL._Core;
using BETMart.BLL.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewBag.ErrorMessage = string.Empty;
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                using HttpClient client = new HttpClient { BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
                HttpContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", model.Email),
                    new KeyValuePair<string, string>("password", model.Password)
                });
                HttpResponseMessage response = await client.PostAsync(Common.Common.APIEndpoint.Login, content);
                var result = await response.Content.ReadAsStringAsync();

                var tokenResponse = JsonConvert.DeserializeObject<Response<TokenResponse>>(result);
                if (tokenResponse.IsSuccessful)
                {
                    var jwtToken = new JwtSecurityToken(tokenResponse.Data.JWToken);
                    var identity = new ClaimsIdentity(jwtToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    identity.AddClaim(new Claim(ClaimTypes.Name, model.Email));
                    identity.AddClaim(new Claim("access_token", tokenResponse.Data.JWToken));

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    HttpContext.User = principal;
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.ErrorMessage = tokenResponse.Message;
                return View("Login");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            using HttpClient client = new HttpClient { BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Common.Common.ContentType.Json));
            //string stringData = JsonConvert.SerializeObject(model);
            //var contentData = new StringContent(stringData, Encoding.UTF8, Common.Common.ContentType.Json);
            HttpContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", model.Email),
                new KeyValuePair<string, string>("password", model.Password),
                new KeyValuePair<string, string>("confirmPassword", model.ConfirmPassword),
                new KeyValuePair<string, string>("firstName", model.FirstName),
                new KeyValuePair<string, string>("lastName", model.LastName)
            });
            HttpResponseMessage response = await client.PostAsync(Common.Common.APIEndpoint.Register, content);
            var result = await response.Content.ReadAsStringAsync();

            var response2 = JsonConvert.DeserializeObject<Response<string>>(result);

            if (response2.IsSuccessful)
            {
                var loginModel = new LoginModel
                {
                    Email = model.Email,
                    Password = model.Password
                };
                return await Login(loginModel);
            }

            ViewBag.ErrorMessage = response2.Message;
            return View("Register");
        }
    }
}
