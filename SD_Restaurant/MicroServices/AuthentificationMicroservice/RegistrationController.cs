using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Restaurant.context;
using SD_Restaurant.Models;

namespace SD_Restaurant.Controllers;

// Класс отвечающий за регистрацию пользователей.
[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private IConfiguration _configuration;
    private ApplicationDbContext _context;

    public RegistrationController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    // Регистрируемся!!
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<User>> RegisterUser(UserRegister userRegister)
    {
        if (_context.User == null)
        {
            return Problem("Entity set 'ApplicationDbContext.User' is null.");
        }

        if (!userRegister.Email.Contains("@"))
        {
            return Problem("Incorrect Email");
        }
        var existing = _context.User.FirstOrDefault(u => u.Email == userRegister.Email);
        if (existing != null)
        {
            return Problem("User with the same Email already exists");
        }

        existing = _context.User.FirstOrDefault(u => u.Username == userRegister.Username);
        if (existing != null)
        {
            return Problem("User with the same Username already exists");
        }

        User user = new();
        var pass = userRegister.Password;
        user.Username = userRegister.Username;
        user.Password_hash = GetHash(pass);
        user.Email = userRegister.Email;
        user.Created_at = DateTime.Now;
        user.Updated_at = DateTime.Now;
        user.Role = "customer";
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return Ok("Successfully registered");
    }

    // Хеш-функция
    private static string GetHash(string password)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
        return result;
    }
}