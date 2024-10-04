using Dapper;
using Microsoft.Data.SqlClient;
using SsoWebApi.Contacts;
using SsoWebApi.Models;

namespace SsoWebApi.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IDapperContext _connection;

        public UserRepository(IDapperContext connection)
        {
            _connection = connection;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using var connection = _connection.CreateDbConnection();
            var query = "SELECT * FROM Users WHERE Username = @Username";
            return await connection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
        }

        public async Task<int> CreateUserAsync(User user)
        {
            using var connection = _connection.CreateDbConnection();
            var query = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash); SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.ExecuteScalarAsync<int>(query, user);
        }
    }
}
