using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        // Метод для получения токена 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(request.Login, request.Password);
                if (user == null)
                {
                    return Unauthorized("Неверный логин или пароль.");
                }

                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(User user)
        {
            // ключ
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // утверждения
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login), // логин
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // id
                new Claim("Role", user.UserGroup.Code) // роль
            }; 

            //  JWT-токен
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])), // Время жизни токена
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
