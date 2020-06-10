using BossSystem.Database;
using BossSystem.Dtos;
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

        public async Task<bool> BuySocksAsync(int ammount)
        {
            const int price = 1;

            User user = await dbContext.Users.Where(u => u. Id == authService.CurrentUser.Id)
                .Include(u => u.Deposits)
                .Include(u => u.Sells)
                .Include(u => u.Buys)
                .FirstOrDefaultAsync();
            int moneyBalance = 0;
            moneyBalance += user.Deposits.Select(d => d.Ammount).Sum();
            moneyBalance -= user.Buys.Select(b => b.Ammount * b.Price).Sum();
            moneyBalance += user.Sells.Select(s => s.Ammount * s.Price).Sum();
            if(moneyBalance < ammount * price)
            {
                throw new BadRequestException("Insufficient funds");
            }
            Buy buy = new Buy
            {
                UserId = user.Id,
                Ammount = ammount,
                Price = price,
                TimestampBought = DateTime.Now
            };
            dbContext.Buys.Add(buy);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DepositMoneyAsync(DepositRequest request)
        {
            Deposit deposit = new Deposit
            {
                UserId = authService.CurrentUser.Id,
                Ammount = request.Ammount,
                TimestampDeposited = DateTime.Now
            };
            dbContext.Deposits.Add(deposit);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public UserSelfDto GetSelf()
        {
            return authService.CurrentUser;
        }

        public async Task<UserDto> GetUserInfoAsync()
        {
            return await dbContext.Users.Where(user => user.Id == authService.CurrentUser.Id)
                .Include(u => u.Deposits)
                .Include(u => u.Buys)
                .Include(u => u.Sells).Select(user =>
                new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MoneyBalance = user.Deposits.Select(s => s.Ammount).Sum() + user.Sells.Select(s => s.Ammount * s.Price).Sum() - user.Buys.Select(s => s.Ammount * s.Price).Sum(),
                    SocksBalance = user.Buys.Select(s => s.Ammount).Sum() - user.Sells.Select(s => s.Ammount).Sum()
                }
             ).FirstOrDefaultAsync();
        }

        public async Task<string> LoginUserAsync(UserLoginRequest request)
        {
            if (request.Email == default || request.Email.Trim().Length == 0)
            {
                throw new BadRequestException("Email can not be empty");
            }
            if (request.Password == default || request.Password.Trim().Length == 0)
            {
                throw new BadRequestException("Password can not be emptya");
            }

            User user = await dbContext.Users.Where(u => u.Email.Equals(request.Email.Trim())).SingleOrDefaultAsync();
            if(user == default)
            {
                throw new BadRequestException("No user registered with that email");
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

        public async Task<bool> SellSocksAsync(int ammount)
        {
            const int price = 1;

            User user = await dbContext.Users.Where(u => u.Id == authService.CurrentUser.Id)
                .Include(u => u.Buys)
                .Include(u => u.Sells)
                .FirstOrDefaultAsync();
            int socksBalance = 0;
            socksBalance += user.Buys.Select(b => b.Ammount).Sum();
            socksBalance -= user.Sells.Select(b => b.Ammount).Sum();
            if (socksBalance < ammount)
            {
                throw new BadRequestException("Not enough socks");
            }
            Sell sell = new Sell
            {
                UserId = user.Id,
                Ammount = ammount,
                Price = price,
                TimestampBought = DateTime.Now
            };
            dbContext.Sells.Add(sell);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
