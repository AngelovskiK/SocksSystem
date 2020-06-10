using BossSystem.Database;
using BossSystem.Dtos;
using BossSystem.Dtos.Requests;
using BossSystem.Models;
using BossSystem.Models.Dbos;
using BossSystem.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOne.Security.Cryptography.BCrypt;

namespace BossSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration Configuration;
        private readonly IAuthService authService;

        public AdminService(ApplicationDbContext applicationDbContext, IConfiguration configuration, IAuthService authService)
        {
            this.dbContext = applicationDbContext;
            this.Configuration = configuration;
            this.authService = authService;
        }

        public async Task<bool> AddUserAsync(AddUserRequest request)
        {
            if (!authService.IsAdmin)
            {
                throw new NotAuthorizedException();
            }
            if(request.FirstName == default || request.FirstName.Trim().Length == 0)
            {
                return false; // to do: add exception
            }
            if (request.LastName == default || request.LastName.Trim().Length == 0)
            {
                return false; // to do: add exception
            }
            if (request.Email == default || request.Email.Trim().Length == 0)
            {
                return false; // to do: add exception
            }
            if (request.Password == default || request.Password.Trim().Length == 0)
            {
                return false; // to do: add exception
            }

            if (dbContext.Users.Where(user => user.Email.Equals(request.Email)).Count() != 0)
            {
                return false; // to do: add exception
            }
            User user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = BCryptHelper.HashPassword(request.Password, BCryptHelper.GenerateSalt(12)),
                Email = request.Email
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            if (!authService.IsAdmin)
            {
                return null; // to do: add exception
            }
            List<User> users = await dbContext.Users.Include(u => u.Deposits).Include(u => u.Buys).Include(u => u.Sells).ToListAsync();
            return await dbContext.Users.Include(u => u.Deposits).Include(u => u.Buys).Include(u => u.Sells).Select(user =>
                new UserDto {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MoneyBalance = user.Deposits.Select(s => s.Ammount).Sum() + user.Sells.Select(s => s.Ammount * s.Price).Sum() - user.Buys.Select(s => s.Ammount * s.Price).Sum(),
                    SocksBalance = user.Buys.Select(s => s.Ammount).Sum() - user.Sells.Select(s => s.Ammount).Sum()
                }
            ).ToListAsync();
        }

        public async Task<string> LoginAdminAsync(AdminLoginRequest request)
        {
            SiteConfiguration passwordConfiguration = await dbContext.SiteConfigurations
                .Where(conf => conf.Key.Equals(Configuration.GetSection("ConfigurationKeys")["AdminPasswordConfigurationKey"]))
                .SingleOrDefaultAsync();
            if(passwordConfiguration.Value.Equals(request.Password))
            {
                return authService.GetTokenForAdmin();
            }else
            {
                return null;
            }
        }
    }
}
