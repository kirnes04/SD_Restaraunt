using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SD_Restaurant.context;
using SD_Restaurant.Models;
using Microsoft.EntityFrameworkCore;

namespace SD_Restaurant.Controllers
{
    // Класс, отвечающий за авторизацию пользователей.
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        private ApplicationDbContext _context;

        public LoginController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // Метод авторизации пользователя.
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            try
            {
                var user = Authenticate(userLogin);

                if (user != null)
                {
                    var token = Generate(user);
                    Session session = new Session
                    {
                        Id = 0, Expires_at = DateTime.Now.AddMinutes(15), User_id = user.Id,
                        Session_token = token
                    };
                    if (_context.Session == null)
                    {
                        return Problem("Entity set 'ApplicationDbContext.Session'  is null.");
                    }

                    _context.Session.Add(session);
                    _context.SaveChanges();
                    return Ok(token);
                }

                return NotFound("User with this email does not exist");
            }
            catch (CustomAttributeFormatException ex)
            {
                return Unauthorized("Incorrect password");
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // Метод генерации JWT токена.
        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Создаем токен
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            string result = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine(result);
            return result;
        }

        // Проверка на существование пользователя с таким электронным адресом и паролем
        private User Authenticate(UserLogin userLogin)
        {
            User currentUser = null;
            foreach (User user in _context.User)
            {
                if (user.Email.ToLower() == userLogin.Email.ToLower())
                {
                    if (user.Password_hash == GetHash(userLogin.Password))
                    {
                        currentUser = user;
                    }
                    else
                    {
                        throw new CustomAttributeFormatException();
                    }
                }
            }

            return currentUser;
        }

        // Хеш-функция для хеширования паролей.
        private static string GetHash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            return result;
        }
    }
}