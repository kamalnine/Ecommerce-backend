using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using Order = Ecommerce.Models.Order;

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
          
            return orders;
        }
        [HttpGet("GetOrderbyId")]
        public IActionResult GetOrderByID(int id)
        {
         
                var order = _context.Order.Where(item => item.OrderID == id);
                if (order.Any())
                {
                    return Ok(order);
                }
                else
                {
                    return NotFound();
                }
           
        }

        [HttpGet("GetOrderBySignupId")]
        public IActionResult GetOrderBySignupId(int id)
        {
            
                var order= _context.Order.Where(item => item.CustomerID == id).ToList();
                if (order.Any())
                {
                    return Ok(order);
                }
                else
                {
                    return NotFound();
                }

        }



        [HttpPut("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, int customerId, DateTime orderDate, DateTime shipDate, string status, decimal totalAmount)
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

      
    }
}
