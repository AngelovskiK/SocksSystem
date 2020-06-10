using BossSystem.Dtos;
using BossSystem.Models;
namespace BossSystem.Services.Auth
{
    public interface IAuthService
    {
        UserSelfDto CurrentUser { get; }
        bool IsAdmin { get; }
        string GetTokenForUser(User user);
        string GetTokenForAdmin();
    }
}
