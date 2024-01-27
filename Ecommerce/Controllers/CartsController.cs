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
    public class CartsController : ControllerBase
    {
        private readonly EcommerceDBContext _context;

        public CartsController(EcommerceDBContext context)
        {
            _context = context;
        }



        [HttpGet("GetCart")]
        public List<Cart> GetCart()
        {
            if (_context.Cart.ToList() == null)
            {
                throw new System.Exception("No Cart item Available");
            }
            List<Cart> carts = _context.Cart.ToList();
            if (carts.Count == 0)
            {
                throw new Exception("No cart item Available");
            }
            return carts;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cart cart)
        {
            try
            {
                _context.Cart.Add(cart);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            return Created("Cart Added", "Cart Added");

        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Cart cart = _context.Cart.Find(id);
                _context.Cart.Remove(cart);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Deletion error. Please try again later.");

            }
            return Ok();
        }
        [HttpPut("UpdateCart/{id}")]
        public async Task<IActionResult> UpdateCart(int id, int customerId, int productId,int quantity)
        {
            try
            {
                var ent = await _context.Cart.FindAsync(id);
                var existingCart = _context.Cart.FirstOrDefault(p => p.CartID == id);

                if (existingCart == null)
                {
                    return NotFound();
                }
                existingCart.CustomerID = customerId;
                existingCart.ProductID= productId;
                existingCart.Quantity = quantity;
              


                _context.Entry(existingCart).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
