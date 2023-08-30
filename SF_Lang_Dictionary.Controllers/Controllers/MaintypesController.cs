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
    public class MaintypesController : ControllerBase
    {
        private readonly SfLangContext _context;

        public MaintypesController(SfLangContext context)
        {
            _context = context;
        }

        // GET: api/Maintypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maintype>>> GetMaintypes()
        {
          if (_context.Maintypes == null)
          {
              return NotFound();
          }
            return await _context.Maintypes.ToListAsync();
        }

        // GET: api/Maintypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Maintype>> GetMaintype(int id)
        {
          if (_context.Maintypes == null)
          {
              return NotFound();
          }
            var maintype = await _context.Maintypes.FindAsync(id);

            if (maintype == null)
            {
                return NotFound();
            }

            return maintype;
        }

        // PUT: api/Maintypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaintype(int id, Maintype maintype)
        {
            if (id != maintype.MtpId)
            {
                return BadRequest();
            }

            _context.Entry(maintype).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintypeExists(id))
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

        // POST: api/Maintypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Maintype>> PostMaintype(Maintype maintype)
        {
          if (_context.Maintypes == null)
          {
              return Problem("Entity set 'SfLangContext.Maintypes'  is null.");
          }
            _context.Maintypes.Add(maintype);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaintype", new { id = maintype.MtpId }, maintype);
        }

        // DELETE: api/Maintypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaintype(int id)
        {
            if (_context.Maintypes == null)
            {
                return NotFound();
            }
            var maintype = await _context.Maintypes.FindAsync(id);
            if (maintype == null)
            {
                return NotFound();
            }

            _context.Maintypes.Remove(maintype);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaintypeExists(int id)
        {
            return (_context.Maintypes?.Any(e => e.MtpId == id)).GetValueOrDefault();
        }
    }
}
