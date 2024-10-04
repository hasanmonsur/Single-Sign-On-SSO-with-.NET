using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SsoWebApi.Contacts;
using SsoWebApi.Models;
using SsoWebApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace SsoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

       // [Authorize]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
           
            // Hash password (use a proper hashing algorithm like BCrypt)
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            var userId = await _userRepository.CreateUserAsync(user);
            return CreatedAtAction(nameof(Register), new { id = userId }, user);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var dbUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (dbUser == null || !BCrypt.Net.BCrypt.Verify(user.PasswordHash, dbUser.PasswordHash))
            {
                return Unauthorized();
            }

            // Generate JWT token
            var token = GenerateJwtToken(dbUser);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
