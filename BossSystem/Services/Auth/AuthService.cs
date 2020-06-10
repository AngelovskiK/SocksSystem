using BossSystem.Dtos;
using BossSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BossSystem.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected UserSelfDto _currentUser;

        public UserSelfDto CurrentUser { 
            get {
                if(_currentUser == default(UserSelfDto))
                {
                    int.TryParse(httpContextAccessor.HttpContext.User.FindFirst("Id").Value, out int id);
                    _currentUser = new UserSelfDto
                    {
                        Id = id,
                        FirstName = httpContextAccessor.HttpContext.User.FindFirst("FirstName").Value,
                        LastName = httpContextAccessor.HttpContext.User.FindFirst("LastName").Value,
                        Email = httpContextAccessor.HttpContext.User.FindFirst("Email").Value
                    };
                }
                return _currentUser;
            } 
        }

        protected bool _isAdmin;

        public bool IsAdmin {
            get
            {
                return httpContextAccessor.HttpContext.User.FindFirst("IsAdmin") != default(Claim) 
                    && httpContextAccessor.HttpContext.User.FindFirst("IsAdmin").Value.Equals("true");
            }
        }

        public string GetTokenForUser(User user)
        {
            DateTime expirationDate = DateTime.Now.AddDays(7);

            var claims = new[]
            {
                  new Claim("Id", user.Id.ToString()),
                  new Claim("FirstName", user.FirstName),
                  new Claim("LastName", user.LastName),
                  new Claim("Email", user.Email),
                  new Claim("ValidUntil", expirationDate.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("SecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration.GetSection("TokenIssuer").Value,
              configuration.GetSection("TokenIssuer").Value,
              claims,
              expires: expirationDate,
              signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetTokenForAdmin()
        {
            DateTime expirationDate = DateTime.Now.AddDays(7);
            var claims = new[]
            {
                  new Claim("ValidUntil", expirationDate.ToString()),
                  new Claim("IsAdmin", "true")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("SecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: configuration.GetSection("TokenIssuer").Value,
              claims: claims,
              expires: expirationDate,
              signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
