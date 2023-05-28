using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_Restaurant.Models;
using SD_Restaurant.context;
using SQLitePCL;

namespace SD_Restaurant.Controllers
{
    // Класс отвечающий за взаимодействие с пользователями (методы доступны только менеджерам)
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        // Получить список пользователей
        [HttpGet]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            if (_context.User == null)
            {
                return NotFound();
            }

            return Ok(await _context.User.ToListAsync());
        }

        // Получить пользователя по токену
        [HttpGet("{token}")]
        public async Task<ActionResult<User>> GetUser(string token)
        {
            if (_context.User == null)
            {
                return NotFound();
            }

            var session = _context.Session.FirstOrDefault(s => s.Session_token == token);
            var user =  _context.User.FirstOrDefault(u => u.Id == session.User_id);

            if (user == null)
            {
                return NotFound("Did not find user with such token");
            }

            return Ok(user);
        }

        // PUT: api/User/5
        // Изменить пользователя по его уникальному айди
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("You can't change user's id");
            }

            var pass = user.Password_hash;
            user.Password_hash = GetHash(pass);
            user.Updated_at = DateTime.Now;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("Did not find user with such id");
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
        }

        // POST: api/User
        // Добавить пользователя
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                if (_context.User == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.User' is null.");
                }

                var pass = user.Password_hash;
                user.Password_hash = GetHash(pass);
                _context.User.Add(user);
                user.Created_at = DateTime.Now;
                user.Updated_at = DateTime.Now;
                var response = await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.ToString());
            }
        }

        // DELETE: api/User/5
        // Удалить пользователя по айди
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            var res = String.Format("Successfully deleted user with id = {id}", id);
            return Ok(res);
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static string GetHash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            return result;
        }
    }
}