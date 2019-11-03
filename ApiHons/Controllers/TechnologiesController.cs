using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiHons.Models;

namespace ApiHons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnologiesController : ControllerBase
    {
        private readonly HonsProjContext _context;

        public TechnologiesController(HonsProjContext context)
        {
            _context = context;
        }

        // GET: api/Technologies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Technologies>>> GetTechnologies()
        {
            return await _context.Technologies.ToListAsync();
        }

        // GET: api/Technologies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Technologies>> GetTechnologies(int id)
        {
            var technologies = await _context.Technologies.FindAsync(id);

            if (technologies == null)
            {
                return NotFound();
            }

            return technologies;
        }

        // PUT: api/Technologies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTechnologies(int id, Technologies technologies)
        {
            if (id != technologies.TechnologyId)
            {
                return BadRequest();
            }

            _context.Entry(technologies).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TechnologiesExists(id))
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

        // POST: api/Technologies
        [HttpPost]
        public async Task<ActionResult<Technologies>> PostTechnologies(Technologies technologies)
        {
            _context.Technologies.Add(technologies);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TechnologiesExists(technologies.TechnologyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTechnologies", new { id = technologies.TechnologyId }, technologies);
        }

        // DELETE: api/Technologies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Technologies>> DeleteTechnologies(int id)
        {
            var technologies = await _context.Technologies.FindAsync(id);
            if (technologies == null)
            {
                return NotFound();
            }

            _context.Technologies.Remove(technologies);
            await _context.SaveChangesAsync();

            return technologies;
        }

        private bool TechnologiesExists(int id)
        {
            return _context.Technologies.Any(e => e.TechnologyId == id);
        }
    }
}
