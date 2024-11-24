using Microsoft.AspNetCore.Mvc;
using SF_Lang_Dictionary.Controllers.Schemas;

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

        [HttpGet("MapToIPA/")]
        public ActionResult<string> MapToIPA(string word)
        {
            // Maps a word to it's IPA pronunciation
            // If the word is null, return an error message with 404 HTTP State
            if (word is null)
            {
                var message = new JsonResult(new { message = "Please specify a word to map" });
                return NotFound(message);
            }

            // Iterate over the word and map each character to it's IPA pronunciation
            var res = new IPARequestResponse()
            {
                response = word.MapToIPA()
            };
            return Ok(res);
        }

        [HttpGet("GetSyllables/")]
        public ActionResult<List<string>> GetSyllables(string word)
        {
            // Maps a word to it's IPA pronunciation
            // If the word is null, return an error message with 404 HTTP State
            if (word is null)
            {
                var message = new JsonResult(new { message = "Please specify a word" });
                return NotFound(message);
            }

            // Iterate over the word and map each character to it's IPA pronunciation
            var res = word.GetSyllables();
            return Ok(res);
        }

        [HttpGet("GetSyllablesString/")]
        public ActionResult<IPARequestResponse> GetSyllablesString(string word)
        {
            // Maps a word to it's IPA pronunciation
            // If the word is null, return an error message with 404 HTTP State
            if (word is null)
            {
                var message = new JsonResult(new { message = "Please specify a word" });
                return NotFound(message);
            }

            // Iterate over the word and map each character to it's IPA pronunciation
            var res = word.GetSyllables();
            var str = string.Join(".", res);
            var response = new IPARequestResponse()
            {
                response = string.Join(".", res)
            };
            return Ok(response);
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
