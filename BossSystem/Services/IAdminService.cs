using BossSystem.Dtos;
using BossSystem.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BossSystem.Services
{
    public interface IAdminService
    {
        Task<string> LoginAdminAsync(AdminLoginRequest request);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
