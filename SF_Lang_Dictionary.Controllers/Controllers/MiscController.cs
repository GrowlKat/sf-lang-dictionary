using Microsoft.AspNetCore.Mvc;
using SF_Lang_Dictionary.Controllers;
namespace SF_Lang_Dictionary.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        // GET: api/<MiscController>
        [HttpGet("GetSpecialChars/{type}")]
        public ActionResult<char[]> GetChars(CharacterType type)
        {
            // Returns the special characters used in the dictionary, depending on the type given
            // If no type is given, throw an error message with 404 HTTP State
            char[] res = type switch
            {
                CharacterType.Writing => Helper.SpecialWritingCharacters,
                CharacterType.IPA => Helper.SpecialIPACharacters,
                CharacterType.All => Helper.SpecialCharacters,
                _ => "".ToCharArray()
            };

            if (type == CharacterType.None)
            {
                // Creates a json object with the error message
                var message = new JsonResult(new { message = "Please specify a type of character to return" });
                return NotFound(message);
            }
            else return res;
        }

        // GET api/<MiscController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MiscController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MiscController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MiscController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public enum CharacterType
    {
        None,
        Writing,
        IPA,
        All
    }
}
