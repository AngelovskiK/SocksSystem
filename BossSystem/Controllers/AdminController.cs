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
    public class AdminController 
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<string> LoginAdminAsync([FromBody] AdminLoginRequest request)
        {
            return await adminService.LoginAdminAsync(request);
        }

        [HttpGet("")]
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await adminService.GetAllUsersAsync();
        }

        [HttpPut("user")]
        public async Task<bool> AddUserAsync([FromBody] AddUserRequest request)
        {
            return await adminService.AddUserAsync(request);
        }
    }
}