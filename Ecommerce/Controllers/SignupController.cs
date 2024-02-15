using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly EcommerceDBContext _context;

         public SignupController(EcommerceDBContext context)
        {
            _context = context;
           

        }

        [HttpGet("GetSignup")]
        public List<Signup> GetSignup()
        {
           
            List<Signup> signup = _context.Signup.ToList();

          
            return signup;

           


        }
        [HttpPost]
        public IActionResult Post([FromBody] Signup signup)
        {

            try
            {
                var existingUser = _context.Signup.FirstOrDefault(u => u.Email == signup.Email);
                /*if (existingUser != null)
                {
                    return BadRequest("Email is already registered. Please login.");

                }*/
                _context.Signup.Add(signup);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();

            }
            return Created("user Added", "user Added");



        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Signup signup = _context.Signup.Find(id);

               /* if (signup == null)
                {
                    return NotFound(); 
                }*/

                signup.Isactive = false;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Deletion error. Please try again later.");
            }
        }

        [HttpPost("CheckEmailExist")]
        public IActionResult CheckEmailExist(string email)
        {
            var user = _context.Signup.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return Ok(1);
            }
            else
            {
                return NotFound(0);
            }
        }
        [HttpGet("GetSignupIdByEmail")]
        public IActionResult GetSignupIdByEmail(string email)
        {

            var user = _context.Signup.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return Ok(user.Signupid);
            }
            else
            {
                return NotFound("0");
            }

        }
        [HttpGet("GetSignupById/{id}")]
        public IActionResult GetSignupById(int id)
        {
            
                var signup = _context.Signup.Find(id);

               

                return Ok(signup);
            
          
        }

       
        [HttpPatch("UpdatePassword")]
        public IActionResult UpdatePassword(string email,string password,string confirmpassword)
        {
            
                var user = _context.Signup.FirstOrDefault(u => u.Email == email);
                user.Password = password;
                user.ConfirmPassword = confirmpassword;
                 _context.SaveChanges();

            return Ok("Password updated successfully.");
        }











    }
}
