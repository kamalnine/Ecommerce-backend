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
            if (_context.Review.ToList() == null)
            {
                throw new System.Exception("No Review Available");
            }
            List<Review> reviews = _context.Review.ToList();
            if (reviews.Count == 0)
            {
                throw new Exception("No Reviews Available");
            }
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
        [HttpPut("UpdateReview/{id}")]
        public async Task<IActionResult> UpdateReview(int id,int productId, int customerId,int rating,string comment)
        {
            try
            {
                var ent = await _context.Review.FindAsync(id);
                var existingReview = _context.Review.FirstOrDefault(p => p.ReviewID == id);

                if (existingReview == null)
                {
                    return NotFound();
                }
                existingReview.ProductID = productId;
                existingReview.CustomerID = customerId;
                
                existingReview.Rating = rating;
                existingReview.Comment = comment;



                _context.Entry(existingReview).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok();

        }


    }
}
