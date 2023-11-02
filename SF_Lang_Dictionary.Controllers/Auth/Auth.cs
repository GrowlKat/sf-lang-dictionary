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
        private readonly IConfiguration config = new ConfigurationBuilder().AddUserSecrets("2f781bee-b429-4f52-a84d-3f36352c147c").Build();
        private readonly SecretManager secretManager = new();
        private string signatureKey = "";
        private string adminUser = "";
        private string adminPassword = "";

        [HttpPost, Route("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            string token;

            try
            {
                signatureKey = secretManager.Client.GetSecret("signatureKey").Value.Value;
                adminUser = secretManager.Client.GetSecret("adminUser").Value.Value;
                adminPassword = secretManager.Client.GetSecret("adminPassword").Value.Value;

                if (string.IsNullOrEmpty(loginDTO.UserName) || string.IsNullOrEmpty(loginDTO.Password))
                    return BadRequest("Username and/or Password not specified");
                
                if (loginDTO.UserName.Equals(adminUser) && loginDTO.Password.Equals(adminPassword))
                {
                    var secretKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(signatureKey) ?? throw new("Signature Key not set")
                    );
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: config.GetValue<string>("issuer"),
                        audience: config.GetValue<string>("audience"),
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
                return BadRequest(e.Message);
            }
            if (token == null)
                return Unauthorized();
            else return Ok(token);
        }
    }
}
