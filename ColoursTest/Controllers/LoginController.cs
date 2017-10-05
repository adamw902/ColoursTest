using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Web.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ColoursTest.Web.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        public LoginController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username != "username" || request.Password != "password")
            {
                return this.BadRequest(new
                {
                    Message = "Username or password is invalid"
                });
            }

            var requestAt = DateTime.UtcNow;
            var expiresAt = requestAt + TokenAuthOption.ExpiresSpan;
            var token = this.GenerateToken(request, expiresAt);

            var loginSuccess = new LoginSuccess
            {
                RequestAt = requestAt,
                ExpiresAt = expiresAt,
                TokenType = TokenAuthOption.TokenType,
                Token = token
            };

            return this.Ok(loginSuccess);
        }

        private string GenerateToken(LoginRequest request, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var identity = new ClaimsIdentity(new GenericIdentity(request.Username, "TokenAuth"), claims);

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = expires,
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
            });

            return handler.WriteToken(securityToken);
        }
    }
}