using BossSystem.Database;
using BossSystem.Dtos.Requests;
using BossSystem.Models;
using BossSystem.Services.Auth;
using DevOne.Security.Cryptography.BCrypt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BossSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAuthService authService;

        public UserService(ApplicationDbContext applicationDbContext, IAuthService authService)
        {
            this.dbContext = applicationDbContext;
            this.authService = authService;
        }

        public async Task<string> LoginUserAsync(UserLoginRequest request)
        {
            if (request.Email == default || request.Email.Trim().Length == 0)
            {
                return null; // to do: add exception
            }
            if (request.Password == default || request.Password.Trim().Length == 0)
            {
                return null; // to do: add exception
            }

            User user = await dbContext.Users.Where(u => u.Email.Equals(request.Email.Trim())).SingleOrDefaultAsync();
            if(user == default)
            {
                return null; // to do: add exception
            }

            if (BCryptHelper.CheckPassword(request.Password, user.Password))
            {
                return authService.GetTokenForUser(user);
            }
            else
            {
                return null;
            }
        }
    }
}
