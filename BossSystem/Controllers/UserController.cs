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

    }
}