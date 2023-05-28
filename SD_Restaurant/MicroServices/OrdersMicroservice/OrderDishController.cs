using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_Restaurant.Models;
using SD_Restaurant.context;

namespace SD_Restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDishController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderDishController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderDish
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDish>>> GetOrderDish()
        {
          if (_context.OrderDish == null)
          {
              return NotFound();
          }
            return await _context.OrderDish.ToListAsync();
        }

        // GET: api/OrderDish/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDish>> GetOrderDish(int id)
        {
          if (_context.OrderDish == null)
          {
              return NotFound();
          }
            var orderDish = await _context.OrderDish.FindAsync(id);

            if (orderDish == null)
            {
                return NotFound();
            }

            return orderDish;
        }

        // PUT: api/OrderDish/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDish(int id, OrderDish orderDish)
        {
            if (id != orderDish.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderDish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDishExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrderDish
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderDish>> PostOrderDish(OrderDish orderDish)
        {
          if (_context.OrderDish == null)
          {
              return Problem("Entity set 'ApplicationDbContext.OrderDish'  is null.");
          }
            _context.OrderDish.Add(orderDish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderDish", new { id = orderDish.Id }, orderDish);
        }

        // DELETE: api/OrderDish/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDish(int id)
        {
            if (_context.OrderDish == null)
            {
                return NotFound();
            }
            var orderDish = await _context.OrderDish.FindAsync(id);
            if (orderDish == null)
            {
                return NotFound();
            }

            _context.OrderDish.Remove(orderDish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDishExists(int id)
        {
            return (_context.OrderDish?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
