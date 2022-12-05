#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeManager.API.Data;
using TimeManager.API.Models;

namespace TimeManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeregistrationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TimeregistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Timeregistrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timeregistration>>> GetTimeregistration()
        {
            return await _context.Timeregistration.ToListAsync();
        }

        // GET: api/Timeregistrations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Timeregistration>> GetTimeregistration(int id)
        {
            var timeregistration = await _context.Timeregistration.FindAsync(id);

            if (timeregistration == null)
            {
                return NotFound();
            }

            return timeregistration;
        }

        // PUT: api/Timeregistrations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeregistration(int id, Timeregistration timeregistration)
        {
            if (id != timeregistration.Id)
            {
                return BadRequest();
            }

            _context.Entry(timeregistration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeregistrationExists(id))
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

        // POST: api/Timeregistrations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Timeregistration>> PostTimeregistration(Timeregistration timeregistration)
        {
            _context.Timeregistration.Add(timeregistration);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimeregistration", new { id = timeregistration.Id }, timeregistration);
        }

        // DELETE: api/Timeregistrations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeregistration(int id)
        {
            var timeregistration = await _context.Timeregistration.FindAsync(id);
            if (timeregistration == null)
            {
                return NotFound();
            }

            _context.Timeregistration.Remove(timeregistration);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TimeregistrationExists(int id)
        {
            return _context.Timeregistration.Any(e => e.Id == id);
        }
    }
}
