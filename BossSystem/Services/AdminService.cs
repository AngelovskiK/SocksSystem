using BossSystem.Database;
using BossSystem.Dtos;
using BossSystem.Dtos.Requests;
using BossSystem.Models.Dbos;
using BossSystem.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            if (!authService.IsAdmin)
            {
                return null; // to do: add exception
            }
            return await dbContext.Users.Select(user =>
                new UserDto {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MoneyBalance = 0,
                    SocksBalance = 0
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
