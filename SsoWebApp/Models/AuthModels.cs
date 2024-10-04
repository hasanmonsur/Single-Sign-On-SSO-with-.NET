namespace SsoWebApp.Models
{
    public class UserRegistrationModel
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class UserLoginModel
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class AuthModels
    {
        public string Username { get; set; }
        public string token { get; set; }
    }

}
