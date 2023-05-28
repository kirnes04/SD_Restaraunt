using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_Restaurant.Models;
using SD_Restaurant.context;

namespace SD_Restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DishController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Dish
        // Получить все доступные блюда (Доступно только менеджеру).
        [HttpGet]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDish()
        {
          if (_context.Dish == null)
          {
              return NotFound();
          }
            return await _context.Dish.ToListAsync();
        }
        
        // Получить меню (Доступно всем).
        [HttpGet]
        [Route("menu")]
        public async Task<ActionResult<IEnumerable<string>>> GetMenu()
        {
            if (_context.Dish == null)
            {
                return NotFound();
            }

            List<string> result = new();
            foreach (var dish in _context.Dish)
            {
                Console.WriteLine(dish.Is_available);
                if (dish.Is_available)
                {
                    result.Add(dish.ToString());
                }
            }

            if (result.Count == 0)
            {
                return NotFound("No available dishes");
            }
            return result;
        }

        // GET: api/Dish/5
        // Получить блюдо по его уникальному номеру (доступно только менеджеру).
        [HttpGet("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
          if (_context.Dish == null)
          {
              return NotFound();
          }
            var dish = await _context.Dish.FindAsync(id);

            if (dish == null)
            {
                return NotFound();
            }

            return dish;
        }

        // PUT: api/Dish/5
        // Изменить блюдо (доступно только менеджеру)
        [HttpPut("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest();
            }
            if (dish.Quantity < 1)
            {
                dish.Is_available = false;
            }
            else
            {
                dish.Is_available = true;
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }

        // POST: api/Dish
        // Добавить блюдо (доступно только менеджеру).
        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Dish>> PostDish(Dish dish)
        {
          if (_context.Dish == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Dish'  is null.");
          }

          if (dish.Quantity < 1)
          {
              dish.Is_available = false;
          }
          else
          {
              dish.Is_available = true;
          }
            _context.Dish.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }

        // DELETE: api/Dish/5
        // Удалить блюдо
        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            if (_context.Dish == null)
            {
                return NotFound();
            }
            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DishExists(int id)
        {
            return (_context.Dish?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
