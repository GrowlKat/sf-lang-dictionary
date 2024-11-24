using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SF_Lang_Dictionary.Controllers.Schemas;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SF_Lang_Dictionary.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth(IConfiguration configuration) : ControllerBase
    {
        private readonly SecretManager secretManager = new();
        private readonly IConfiguration configuration = configuration;
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
                // Get the value of variables from Azure Key Vault, or local if it's development environment
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (env is not null && env.Equals("Production"))
                {
                    signatureKey = secretManager.Client.GetSecret("signatureKey").Value.Value ?? throw new("Signature Key not found");
                    adminUser = secretManager.Client.GetSecret("adminUser").Value.Value ?? throw new("Admin User not found");
                    adminPassword = secretManager.Client.GetSecret("adminPassword").Value.Value ?? throw new("Admin Password not found");
                    issuer = secretManager.Client.GetSecret("issuer").Value.Value ?? throw new("Issuer not found");
                    audience = secretManager.Client.GetSecret("audience").Value.Value ?? throw new("Audience not found");
                }
                else
                {
                    signatureKey = configuration.GetValue<string>("signatureKey") ?? throw new("Signature Key not found");
                    adminUser = configuration.GetValue<string>("adminUser") ?? throw new("Admin User not found");
                    adminPassword = configuration.GetValue<string>("adminPassword") ?? throw new("Admin Password not found");
                    issuer = configuration.GetValue<string>("issuer") ?? throw new("Issuer not found");
                    audience = configuration.GetValue<string>("audience") ?? throw new("Audience not found");
                }

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
                        claims: [],
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signinCredentials
                    );
                    token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                }
                else return Unauthorized("Wrong User or Password providen, try again");
            }
            catch(Exception e)
            {
                // Return error message if any of the variables are not found
                return BadRequest(e.Message);
            }
            // If everything is correct, return the JWT token, otherwise return an Unauthorized HTTP State
            if (token == null)
                return Unauthorized("Something wrong happened while creating authorizaton token, please try again");
            else return Ok(token);
        }
    }
}
