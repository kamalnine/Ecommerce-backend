using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly EcommerceDBContext _context;

        public WishlistController(EcommerceDBContext context)
        {
            _context = context;
        }

        // POST: api/Wishlist
        [HttpPost]
        public async Task<ActionResult<Wishlist>> PostWishlist(Wishlist wishlist)
        {
            _context.Wishlist.Add(wishlist);
            await _context.SaveChangesAsync();

            return Ok(wishlist);
        }

        // DELETE: api/Wishlist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {


            var wishlist =_context.Wishlist.FirstOrDefault(p => p.ProductID == id);
            _context.Wishlist.Remove(wishlist);
             _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Wishlist/Customer/5
        [HttpGet("Customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetWishlistByCustomer(int customerId)
        {
            var wishlist = await _context.Wishlist.Where(w => w.CustomerID == customerId).ToListAsync();

            return wishlist;
        }
    }
}
