using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF_Lang_Dictionary.Controllers.Schemas;
using SF_Lang_Dictionary.Models;

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

        [HttpGet("GetAll/")]
        public async Task<ActionResult<IEnumerable<Rootword>>> GetAllRootwords([FromQuery] bool sortAlphabetically, [FromQuery] string lang = "sf")
        {
            if (_context.Rootwords == null)
            {
                return NotFound();
            }

            var result = await _context.Rootwords.ToListAsync();

            // Sorts the result alphabetically depending on the language
            if (lang == "sf")
            {
                if (sortAlphabetically) result.Sort((a, b) => string.Compare(a.Rootword1, b.Rootword1, StringComparison.Ordinal));
            }
            else if (Helper.AvailableLanguages.ContainsKey(lang))
            {
                if (sortAlphabetically) result.Sort((a, b) => string.Compare(a.Meaning, b.Meaning, StringComparison.Ordinal));
            }
            else return BadRequest("Language not supported");

            return result;
        }

        [HttpGet("SearchWords/")]
        public async Task<ActionResult<IEnumerable<Rootword>>> SearchWords([FromQuery] string search, [FromQuery] bool sortAlphabetically, [FromQuery] string lang = "sf")
        {
            if (_context.Rootwords == null)
            {
                return NotFound();
            }

            List<Rootword> result;

            // Searches for the word in the rootword or meaning depending on the language
            if (lang == "sf")
            {
                // Searches for the rootword and sorts the result alphabetically
                result = await _context.Rootwords.Select(r => r).Where(r => r.Rootword1 != null && r.Rootword1.Contains(search)).ToListAsync();
                if (sortAlphabetically) result.Sort((a, b) => string.Compare(a.Rootword1, b.Rootword1, StringComparison.Ordinal));
            }
            else if (Helper.AvailableLanguages.ContainsKey(lang))
            {
                // Searches for the rootword and sorts the result alphabetically
                result = await _context.Rootwords.Select(r => r).Where(r => r.Meaning != null && r.Meaning.Contains(search)).ToListAsync();
                if (sortAlphabetically) result.Sort((a, b) => string.Compare(a.Meaning, b.Meaning, StringComparison.Ordinal));
            }
            else return BadRequest("Language not supported");

            return result;
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

        // GET: api/Rootwords/GetByWord/y
        [HttpGet("GetByWord/")]
        public async Task<ActionResult<Rootword>> GetRootwordByWord([FromQuery] string word, [FromQuery] string lang = "sf")
        {
            // Validates the parameters
            if (_context.Rootwords == null) return NotFound();
            if (string.IsNullOrEmpty(word)) return BadRequest("Please set a word to find");
            if (!Helper.AvailableLanguages.ContainsKey(lang))  return BadRequest("Language not supported");

            Rootword? rootword;

            // Tries to get an exact match of the word searched

            // Search by it's rootword if the language is selenian
            if (lang == "sf")
            {
                rootword = await _context.Rootwords.FirstOrDefaultAsync(r => r.Rootword1 != null && r.Rootword1.Equals(word));
                if (rootword == null)
                {
                    // If an exact match is not found, tries to search a word that contains the parameter string, if it's not found, returns a Not Found HTTP State
                    rootword = await _context.Rootwords.FirstOrDefaultAsync(r => r.Rootword1 != null && r.Rootword1.Contains(word));
                    return rootword is not null ? rootword : NotFound("Word not found, please try with another search");
                }
            }
            // Search by it's meaning if the language is not selenian, first on it's english meaning and then translates it
            else
            {
                rootword = await _context.Rootwords.FirstOrDefaultAsync(r => r.Meaning != null && r.Meaning.ToLower().Equals(word));
                if (rootword == null)
                {
                    // If an exact match is not found, tries to search a word that contains the parameter string, if it's not found, returns a Not Found HTTP State
                    rootword = await _context.Rootwords.FirstOrDefaultAsync(r => r.Meaning != null && r.Meaning.Contains(word));
                    return rootword is not null ? rootword : NotFound("Word not found, please try with another search");
                }
            }

            return rootword;
        }

        // POST: api/Rootwords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UploadWord/"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Rootword>> PostRootword(Rootword rootword)
        {
            Console.WriteLine("Entrando");
            // If JWT authentication fails, returns an Unauthorized HTTP State
            if (!ModelState.IsValid) return Unauthorized("Please enter a valid token");
            if (_context.Rootwords == null)
            {
                return Problem("Entity set 'SfLangContext.Rootwords' is null.");
            }
            _context.Rootwords.Add(rootword);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRootword", new { id = rootword.RootId }, rootword);
        }

        [HttpPost("SearchByTags/")]
        public async Task<ActionResult<IEnumerable<Rootword>>> GetRootwordsByTag(TagsRequest request, [FromQuery] bool sortAlphabetically = true, bool exclusiveSearch = true, [FromQuery] string lang = "sf")
        {
            if (_context.Rootwords == null)
            {
                return NotFound();
            }

            List<Rootword> result = [];
            var tags = request.tags;

            // Searches for all the rootwords that contain any of the tags provided
            foreach (var t in tags)
            {
                Console.WriteLine($"Searching for Tag: {t}\n");
                var newData = await _context.Rootwords.Select(r => r).Where(r => r.Tags != null && r.Tags.ToLower().Contains(t)).ToListAsync();
                foreach (var n in newData) result.Add(n);
            }

            // Filters the result to return only rootwords that have all the tags provided, if exclusiveSearch is set to true
            if (exclusiveSearch)
            {
                List<Rootword> temp = [];
                foreach (var r in result)
                {
                    var t = r.Tags?.ToLower().Split(", "); // Splits the tags string into a list of tags
                    int tagCount = 0; // Counter to check if all the tags are present in the rootword
                    if (t != null)
                    {
                        foreach (var tag in t)
                        {
                            // If the tag is present in the list of tags provided, increments the counter
                            if (tags != null && tags.Contains(tag))
                            {
                                tagCount++;
                                continue;
                            }
                        }
                    }

                    // If the counter is equal to the amount of tags provided, adds the rootword to the result
                    if (tags != null && tagCount == tags.Count)
                    {
                        temp.Add(r);
                        continue;
                    }
                }
                result = temp; // Updates the result with the filtered list
            }

            // Sorts the result alphabetically depending on the language
            if (lang == "sf")
            {
                if (sortAlphabetically) result.Sort((a, b) => string.Compare(a.Rootword1, b.Rootword1, StringComparison.Ordinal));
            }
            else if (Helper.AvailableLanguages.ContainsKey(lang))
            {
                if (sortAlphabetically) result.Sort((a, b) => string.Compare(a.Meaning, b.Meaning, StringComparison.Ordinal));
            }
            else return BadRequest("Language not supported");

            result = result.Distinct().ToList(); // Remove duplicates

            return result;
        }

        // PUT: api/Rootwords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        // DELETE: api/Rootwords/5
        [HttpDelete("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
