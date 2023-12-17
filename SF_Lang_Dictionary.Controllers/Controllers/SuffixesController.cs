using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF_Lang_Dictionary.Models;

namespace SF_Lang_Dictionary.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuffixesController : ControllerBase
    {
        private readonly SfLangContext _context;

        public SuffixesController(SfLangContext context)
        {
            _context = context;
        }

        // GET: api/Suffixes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Suffix>>> GetSuffixes()
        {
            if (_context.Suffixes == null)
            {
                return NotFound();
            }
            return await _context.Suffixes.ToListAsync();
        }

        // GET: api/Suffixes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Suffix>> GetSuffix(int id)
        {
            if (_context.Suffixes == null)
            {
                return NotFound();
            }
            var suffix = await _context.Suffixes.FindAsync(id);

            if (suffix == null)
            {
                return NotFound();
            }

            return suffix;
        }

        // GET: api/Suffixes/5
        [HttpGet("GetByDeclension/{declension}")]
        public async Task<ActionResult<IEnumerable<Suffix>>> GetSuffixByDeclination(Declension declension)
        {
            if (_context.Suffixes == null)
            {
                return NotFound();
            }

            // Gets all the suffixes and their maintypes and subtypes
            // Only gets the suffixes that matches the declension and only gets the maintype and subtype ids and names
            var res = await _context.Suffixes
                    .Select(s => new Suffix()
                    {
                        SfxId = s.SfxId, // Id of the suffix
                        Suffix1 = s.Suffix1, // Suffix
                        MtpId = s.MtpId, // Id of the maintype
                        Mtp = new Maintype() // Sets the maintype as a new object
                        {
                            MtpId = s.Mtp != null ? s.Mtp.MtpId : default, // Id of the maintype
                            Maintype1 = s.Mtp != null ? s.Mtp.Maintype1 : null // Name of the maintype
                        },
                        Stp = new Subtype() // Sets the subtype as a new object
                        {
                            StpId = s.Stp != null ? s.Stp.StpId : default, // Id of the subtype
                            Subtype1 = s.Stp != null ? s.Stp.Subtype1 : null // Name of the subtype
                        }
                    })
                    .Where(s => s.MtpId == Convert.ToInt32(declension)) // Only gets the suffixes that matches the declension
                    .ToListAsync();

            // If the result is null, return not found
            if (res == null) return NotFound("Cases not found");

            return res;
        }

        // PUT: api/Suffixes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutSuffix(int id, Suffix suffix)
        {
            if (id != suffix.SfxId)
            {
                return BadRequest();
            }

            _context.Entry(suffix).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuffixExists(id))
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

        // POST: api/Suffixes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Suffix>> PostSuffix(Suffix suffix)
        {
          if (_context.Suffixes == null)
          {
              return Problem("Entity set 'SfLangContext.Suffixes'  is null.");
          }
            _context.Suffixes.Add(suffix);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSuffix", new { id = suffix.SfxId }, suffix);
        }

        // DELETE: api/Suffixes/5
        [HttpDelete("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteSuffix(int id)
        {
            if (_context.Suffixes == null)
            {
                return NotFound();
            }
            var suffix = await _context.Suffixes.FindAsync(id);
            if (suffix == null)
            {
                return NotFound();
            }

            _context.Suffixes.Remove(suffix);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SuffixExists(int id)
        {
            return (_context.Suffixes?.Any(e => e.SfxId == id)).GetValueOrDefault();
        }
    }
}
