using SsoWebApi.Models;

namespace SsoWebApi.Contacts
{
    public interface IUserRepository
    {
        Task<int> CreateUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
    }
}