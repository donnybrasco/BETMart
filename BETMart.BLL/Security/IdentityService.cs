using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BETMart.BLL._Core;
using BETMart.BLL.Notifications;
using BETMart.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BETMart.BLL.Security
{
    public interface IIdentityService
    {
        Task<Response<TokenResponse>> GetTokenAsync(LoginModel model, string ipAddress);

        Task<Response<string>> RegisterAsync(RegisterModel model, string origin);

        Task<Response<string>> ConfirmEmailAsync(string userId, string code);

        Task ForgotPassword(ForgotPasswordModel model, string origin);

        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
    }
    public class IdentityService 
        : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IMailService _mailService;

        public IdentityService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            SignInManager<User> signInManager, IMailService mailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        public async Task<Response<TokenResponse>> GetTokenAsync(LoginModel request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NullReferenceException($"No Accounts Registered with {request.Email}.");
            }
            var result = await _signInManager.PasswordSignInAsync(user.Email, request.Password, false, lockoutOnFailure: false);
            //if (!user.EmailConfirmed)
            //{
                //TODO throw new Exception($"Email is not confirmed for '{model.Email}'.");
            //}

            //if (!user.IsActive)
            //{
                //TODO throw new Exception($"Account for '{model.Email}' is not active. Please contact the Administrator.");
            //}

            if (!result.Succeeded)
            {
                throw new Exception($"Wrong Credentials entered for '{request.Email}'.");
            }
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, ipAddress);
            var response = new TokenResponse
            {
                Id = user.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                IssuedOn = jwtSecurityToken.ValidFrom.ToLocalTime(),
                ExpiresOn = jwtSecurityToken.ValidTo.ToLocalTime(),
                Email = user.Email
            };
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            return await Response<TokenResponse>.SuccessAsync(response, "Authenticated");
        }

        private async Task<JwtSecurityToken> GenerateJWToken(User user, string ipAddress)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("first_name", user.FirstName),
                new Claim("last_name", user.LastName),
                new Claim("full_name", $"{user.FirstName} {user.LastName}"),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);
            return JWTGeneration(claims);
        }

        private JwtSecurityToken JWTGeneration(IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<Response<string>> RegisterAsync(RegisterModel model, string origin)
        {
            var userWithSameUserName = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameUserName != null)
            {
                throw new Exception($"Email Address '{model.Email}' is already taken.");
            }
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, BETMart.Common.Common.Roles.Basic.ToString());
                    var verificationUri = await SendVerificationEmail(user, origin);
                    //TODO: Attach Email Service here and configure it via appsettings
                    await _mailService.SendAsync(new MailModel() { From = "mail@betmart.co.za", To = user.Email, Body = $"Please confirm your account by <a href='{verificationUri}'>clicking here</a>.", Subject = "Confirm Registration" });
                    return await Response<string>.SuccessAsync(user.Id, message: $"User Registered. Confirmation Mail has been delivered to your Mailbox. (DEV) Please confirm your account by visiting this URL {verificationUri}");
                }
                else
                {
                    throw new Exception($"{result.Errors}");
                }
            }
            else
            {
                throw new Exception($"Email {model.Email } is already registered.");
            }
        }

        private async Task<string> SendVerificationEmail(User user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/identity/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return await Response<string>.SuccessAsync(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/identity/token endpoint to generate JWT.");
            }
            else
            {
                throw new Exception($"An error occurred while confirming {user.Email}.");
            }
        }

        public async Task ForgotPassword(ForgotPasswordModel model, string origin)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "api/identity/reset-password/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailRequest = new MailModel()
            {
                Body = $"You reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password",
            };
            //await _mailService.SendAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new Exception($"No Accounts Registered with {model.Email}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Response<string>.Success(model.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new Exception($"Error occured while reseting the password.");
            }
        }
    }
}
