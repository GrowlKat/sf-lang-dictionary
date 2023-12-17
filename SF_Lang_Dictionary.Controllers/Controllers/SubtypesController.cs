using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF_Lang_Dictionary;
using SF_Lang_Dictionary.Models;

namespace SF_Lang_Dictionary.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtypesController : ControllerBase
    {
        private readonly SfLangContext _context;

        public SubtypesController(SfLangContext context)
        {
            _context = context;
        }

        // GET: api/Subtypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subtype>>> GetSubtypes()
        {
          if (_context.Subtypes == null)
          {
              return NotFound();
          }
            return await _context.Subtypes.ToListAsync();
        }

        // GET: api/Subtypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subtype>> GetSubtype(int id)
        {
          if (_context.Subtypes == null)
          {
              return NotFound();
          }
            var subtype = await _context.Subtypes.FindAsync(id);

            if (subtype == null)
            {
                return NotFound();
            }

            return subtype;
        }

        // PUT: api/Subtypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutSubtype(int id, Subtype subtype)
        {
            if (id != subtype.StpId)
            {
                return BadRequest();
            }

            _context.Entry(subtype).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubtypeExists(id))
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

        // POST: api/Subtypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Subtype>> PostSubtype(Subtype subtype)
        {
          if (_context.Subtypes == null)
          {
              return Problem("Entity set 'SfLangContext.Subtypes'  is null.");
          }
            _context.Subtypes.Add(subtype);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubtype", new { id = subtype.StpId }, subtype);
        }

        // DELETE: api/Subtypes/5
        [HttpDelete("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteSubtype(int id)
        {
            if (_context.Subtypes == null)
            {
                return NotFound();
            }
            var subtype = await _context.Subtypes.FindAsync(id);
            if (subtype == null)
            {
                return NotFound();
            }

            _context.Subtypes.Remove(subtype);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubtypeExists(int id)
        {
            return (_context.Subtypes?.Any(e => e.StpId == id)).GetValueOrDefault();
        }
    }
}
