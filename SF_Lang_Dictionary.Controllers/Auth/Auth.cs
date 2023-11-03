using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SF_Lang_Dictionary.Controllers.Schemas;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SF_Lang_Dictionary.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly SecretManager secretManager = new();
        private string signatureKey = "";
        private string adminUser = "";
        private string adminPassword = "";
        private string issuer = "";
        private string audience = "";

        [HttpPost, Route("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            string token;

            try
            {
                // Get the value of variables from Azure Key Vault
                signatureKey = secretManager.Client.GetSecret("signatureKey").Value.Value;
                adminUser = secretManager.Client.GetSecret("adminUser").Value.Value;
                adminPassword = secretManager.Client.GetSecret("adminPassword").Value.Value;
                issuer = secretManager.Client.GetSecret("issuer").Value.Value;
                audience = secretManager.Client.GetSecret("audience").Value.Value;

                // Check if username and password are specified
                if (string.IsNullOrEmpty(loginDTO.UserName) || string.IsNullOrEmpty(loginDTO.Password))
                    return BadRequest("Username and/or Password not specified");
                
                // Check if username and password are correct
                if (loginDTO.UserName.Equals(adminUser) && loginDTO.Password.Equals(adminPassword))
                {
                    // Create JWT token by signing it with the signature key
                    var secretKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(signatureKey) ?? throw new("Signature Key not set")
                    );
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: issuer,
                        audience: audience,
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signinCredentials
                    );
                    token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                }
                else return Unauthorized();
            }
            catch(Exception e)
            {
                // Return error message if any of the variables are not found
                return BadRequest(e.Message);
            }
            // If everything is correct, return the JWT token, otherwise return an Unauthorized HTTP State
            if (token == null)
                return Unauthorized();
            else return Ok(token);
        }
    }
}
