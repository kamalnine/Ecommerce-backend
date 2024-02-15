using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly EcommerceDBContext _context;

        public ReviewController(EcommerceDBContext context)
        {
            _context = context;
        }

        [HttpGet("GetReview")]
        public List<Review> GetReview()
        {
            
            List<Review> reviews = _context.Review.ToList();
          
            return reviews;
        }


        [HttpPost]
        public IActionResult Post([FromBody] Review review)
        {
            try
            {
                _context.Review.Add(review);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            return Created("Review Added", "Review Added");

        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Review review= _context.Review.Find(id);
                review.Isactive = false;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Deletion error. Please try again later.");

            }
            return Ok();
        }
        

    }
}
