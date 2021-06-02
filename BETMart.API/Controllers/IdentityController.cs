using System.Threading.Tasks;
using BETMart.BLL.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BETMart.API.Controllers
{
    public class IdentityController
        : ControllerBase<IdentityController>
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            this._identityService = identityService;
        }

        /// <summary>
        /// Generates a JSON Web Token for a valid combination of emailId and password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync(LoginModel model)
        {
            var ipAddress = GenerateIPAddress();
            var token = await _identityService.GetTokenAsync(model, ipAddress);
            return Ok(token);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.RegisterAsync(model, origin));
        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _identityService.ConfirmEmailAsync(userId, code));
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            await _identityService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            return Ok(await _identityService.ResetPassword(model));
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
