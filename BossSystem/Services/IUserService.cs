using BossSystem.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BossSystem.Services
{
    public interface IUserService
    {
        Task<string> LoginUserAsync(UserLoginRequest request);
    }
}
