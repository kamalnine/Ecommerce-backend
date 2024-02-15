
using Ecommerce.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly EcommerceDBContext _context;

        public CartController(EcommerceDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();

            return Ok(cart);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Cart.FirstAsync(p => p.ProductID == id);

            

            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("Customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Cart>>>GetCartByCustomer(int customerId)
        {
            var cart = await _context.Cart.Where(w => w.CustomerID == customerId).ToListAsync();

            return cart;
        }

        [HttpDelete("Customer/{customerId}")]
        public async Task<IActionResult> DeleteCartByCustomer(int customerId)
        {
            var carts = await _context.Cart.Where(c => c.CustomerID == customerId).ToListAsync();

           

            _context.Cart.RemoveRange(carts);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("UpdateQuantity/{productId}")]
        public async Task<IActionResult> UpdateQuantity(int productId, [FromBody] int quantity)
        {
            var cartItem = await _context.Cart.FirstOrDefaultAsync(c => c.ProductID == productId);

            

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("UpdateVariant/{productId}")]
        public async Task<IActionResult> UpdateVariant(int productId, [FromBody] string variant)
        {
            var cartItem = await _context.Cart.FirstOrDefaultAsync(c => c.ProductID == productId);

            
            cartItem.Variant = variant;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
