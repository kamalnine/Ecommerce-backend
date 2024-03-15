
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
        public ActionResult DeleteCart(int id)
        {
            var cart =  _context.Cart.First(p => p.ProductID == id);

            

            _context.Cart.Remove(cart);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("Customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Cart>>>GetCartByCustomer(int customerId)
        {
            var cart = await _context.Cart.Where(w => w.CustomerID == customerId).ToListAsync();

            return cart;
        }

        [HttpDelete("Customer/{customerId}")]
        public ActionResult DeleteCartByCustomer(int customerId)
        {
            var carts =  _context.Cart.Where(c => c.CustomerID == customerId).ToList();

           

            _context.Cart.RemoveRange(carts);
             _context.SaveChanges();

            return NoContent();
        }
        [HttpPut("UpdateQuantity/{productId}")]
        public ActionResult UpdateQuantity(int productId, [FromBody] int quantity)
        {
            var cartItem = _context.Cart.FirstOrDefault(c => c.ProductID == productId);

            

            cartItem.Quantity = quantity;
             _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("UpdateVariant/{productId}")]
        public ActionResult UpdateVariant(int productId, [FromBody] string variant)
        {
            var cartItem =  _context.Cart.FirstOrDefault(c => c.ProductID == productId);

            
            cartItem.Variant = variant;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
