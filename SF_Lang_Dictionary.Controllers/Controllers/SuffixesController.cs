using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF_Lang_Dictionary;
using SF_Lang_Dictionary.Controllers.Schemas;

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
        [HttpGet("GetByDeclination/{declination}")]
        public async Task<ActionResult<IEnumerable<Suffix>>> GetSuffixByDeclination(Declination declination)
        {
            if (_context.Suffixes == null)
            {
                return NotFound();
            }

            List<SuffixSchema> sfxs = new();
            var res = await _context.Suffixes.Where(s => s.MtpId == Convert.ToInt32(declination)).ToListAsync();

            if (res == null)
            {
                return NotFound("Cases not found");
            }
            /*else
            {
                res = _context.Suffixes.Include(sfx => sfx.Mtp).ToList();
                
                for (int i = 0; i < res.Count; i++)
                {
                    sfxs.Add(new() { SfxId = res[i].SfxId, Suffix1 = res[i].Suffix1, Mtp = res[i].Mtp?.Maintype1, Stp = res[i].Stp?.Subtype1 });
                    Console.WriteLine(sfxs[i].SfxId);
                    Console.WriteLine(sfxs[i].Suffix1);
                    Console.WriteLine(sfxs[i].Mtp);
                    Console.WriteLine(sfxs[i].Stp);
                }
            }*/

            return res;
        }

        // PUT: api/Suffixes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
        [HttpPost]
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
        [HttpDelete("{id}")]
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
