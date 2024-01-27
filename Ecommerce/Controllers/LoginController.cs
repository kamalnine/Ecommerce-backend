using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly EcommerceDBContext _context;
        IConfiguration configuration;

        public LoginController(EcommerceDBContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        [HttpPost]

        public IActionResult Post(string email, string password)
        {

            var user = _context.Signup.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return Unauthorized();
            }

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            var signingCredentials = new SigningCredentials(
                                    new SymmetricSecurityKey(key),
                                    SecurityAlgorithms.HmacSha512Signature
                                );
            var claims = new List<Claim>
{
      new Claim(JwtRegisteredClaimNames.Email, email),
};
            string userRole = null;


            if (email == "kamal@gmail.com" || email == "kamalsutte786@gmail.com")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                userRole = "Admin";
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                userRole = "Customer";
            }
           
            
            var expires = DateTime.UtcNow.AddMinutes(10);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return Ok(new { Token = jwtToken, Role = userRole });



        }

        [HttpPost("GetName")]

        public IActionResult GetName(string email, string password)
        {
            var user = _context.Signup.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user.Name);

        }
        [HttpPost("GetDetails")]
        public IActionResult Post(string email)
        {
            var user = _context.Signup.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return Ok("Not Authorized");
            }

            return Ok("Authorized");
        }
    }
}
