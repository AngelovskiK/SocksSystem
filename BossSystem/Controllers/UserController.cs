using BossSystem.Dtos;
using BossSystem.Dtos.Requests;
using BossSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BossSystem.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<string> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            return await userService.LoginUserAsync(request);
        }

        [HttpPost("deposit")]
        public async Task<bool> DepositMoneyAsync([FromBody] DepositRequest request)
        {
            return await userService.DepositMoneyAsync(request);
        }

        [HttpPost("buy/{ammount}")]
        public async Task<bool> BuySocksAsync([FromRoute] int ammount)
        {
            return await userService.BuySocksAsync(ammount);
        }

        [HttpPost("sell/{ammount}")]
        public async Task<bool> SellSocksAsync([FromRoute] int ammount)
        {
            return await userService.SellSocksAsync(ammount);
        }

        [HttpGet("info")]
        [Authorize]
        public async Task<UserDto> GetUserInfoAsync()
        {
            return await userService.GetUserInfoAsync();
        }

        [HttpGet("self")]
        [Authorize]
        public UserSelfDto GetSelf()
        {
            return userService.GetSelf();
        }
    }
}