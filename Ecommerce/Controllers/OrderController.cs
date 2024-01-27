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
    public class OrderController : ControllerBase
    {
        private readonly EcommerceDBContext _context;

        public OrderController(EcommerceDBContext context)
        {
            _context = context;
        }

        
        [HttpGet("GetOrder")]
        public List<Order> GetOrder()
        {
            if (_context.Order.ToList() == null)
            {
                throw new System.Exception("No Orders Available");
            }
            List<Order> orders = _context.Order.ToList();
            if (orders.Count == 0)
            {
                throw new Exception("No Orders Available");
            }
            return orders;
        }




        [HttpPut("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, int customerId, DateTime orderDate, DateTime shipDate, string status, decimal totalAmount)
        {
            try
            {
                var ent = await _context.Order.FindAsync(id);
                var existingOrder = _context.Order.FirstOrDefault(p => p.OrderID == id);

                if (existingOrder == null)
                {
                    return NotFound();
                }
                existingOrder.CustomerID = customerId;
                existingOrder.OrderDate= orderDate;
                existingOrder.ShipDate = shipDate;
                existingOrder.Status = status;
                existingOrder.TotalAmount = totalAmount;
               

                _context.Entry(existingOrder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok();

        }

        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            try
            {
                _context.Order.Add(order);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            return Created("Order Added", order);

        }

      
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Order order= _context.Order.Find(id);
                order.Isactive = false;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Deletion error. Please try again later.");

            }
            return Ok();
        }

        private bool OrderExists(int id)
        {
            return (_context.Order?.Any(e => e.OrderID == id)).GetValueOrDefault();
        }
    }
}
