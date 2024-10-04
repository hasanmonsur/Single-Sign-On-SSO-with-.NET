using SsoWebApp.Models;

namespace SsoWebApp.Contacts
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UserLoginModel model);
        Task<string> RegisterAsync(UserRegistrationModel model, string token);
    }
}