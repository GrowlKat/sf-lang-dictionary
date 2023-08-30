using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF_Lang_Dictionary;

namespace SF_Lang_Dictionary.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RootwordsController : ControllerBase
    {
        private readonly SfLangContext _context;

        public RootwordsController(SfLangContext context)
        {
            _context = context;
        }

        // GET: api/Rootwords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rootword>>> GetRootwords()
        {
            if (_context.Rootwords == null)
            {
                return NotFound();
            }
            return await _context.Rootwords.ToListAsync();
        }

        // GET: api/Rootwords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rootword>> GetRootword(int id)
        {

            if (_context.Rootwords == null)
            {
                return NotFound();
            }
            var rootword = await _context.Rootwords.FindAsync(id);

            if (rootword == null)
            {
                return NotFound();
            }

            return rootword;
        }

        // PUT: api/Rootwords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRootword(int id, Rootword rootword)
        {
            if (id != rootword.RootId)
            {
                return BadRequest();
            }

            _context.Entry(rootword).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RootwordExists(id))
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

        // POST: api/Rootwords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rootword>> PostRootword(Rootword rootword)
        {
          if (_context.Rootwords == null)
          {
              return Problem("Entity set 'SfLangContext.Rootwords'  is null.");
          }
            _context.Rootwords.Add(rootword);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRootword", new { id = rootword.RootId }, rootword);
        }

        // DELETE: api/Rootwords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRootword(int id)
        {
            if (_context.Rootwords == null)
            {
                return NotFound();
            }
            var rootword = await _context.Rootwords.FindAsync(id);
            if (rootword == null)
            {
                return NotFound();
            }

            _context.Rootwords.Remove(rootword);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RootwordExists(int id)
        {
            return (_context.Rootwords?.Any(e => e.RootId == id)).GetValueOrDefault();
        }
    }
}
