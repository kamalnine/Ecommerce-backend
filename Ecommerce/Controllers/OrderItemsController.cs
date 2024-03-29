﻿using System;
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
    public class OrderItemsController : ControllerBase
    {
        private readonly EcommerceDBContext _context;

        public OrderItemsController(EcommerceDBContext context)
        {
            _context = context;
        }

        // GET: api/OrderItems
        [HttpGet("GetOrderItems")]
        public List<OrderItems> GetOrderItems()
        {
            List<OrderItems> products = _context.OrderItems.ToList();
           
            return products;
        }
        [HttpGet("GetOrderBySignupId")]
        public IActionResult GetOrderItemsBySignupId(int id)
        {
            try
            {
                var orderItems = _context.OrderItems.Where(item => item.signupId == id).ToList();
                DateTime today = DateTime.Today;
                if (orderItems.Any())
                {
                    foreach (var item in orderItems)
                    {
                        // Check if shipDate is greater than today's date
                        if (item.ShipDate.Date > today)
                        {
                            // Update the order status to "Delivered"
                            var order = _context.Order.FirstOrDefault(o => o.OrderID == item.OrderId);
                            if (order != null)
                            {
                                order.Status = "Delivered";
                            }
                        }
                    }
                    _context.SaveChanges();
                    return Ok(orderItems);
                }
                else
                {
                    return NotFound();
                }

              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving order items: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItems>> GetOrderItem(int id)
        {
       
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItems orderItem)
        {
            if (id != orderItem.OrderItemID)
            {
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            
                await _context.SaveChangesAsync();
            
           
          

            return NoContent();
        }

        
        [HttpPost]
        public async Task<ActionResult<OrderItems>> PostOrderItem(OrderItems orderItem)
        {
          if (_context.OrderItems == null)
          {
              return Problem("Entity set 'EcommerceDBContext.OrderItems'  is null.");
          }
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            var product = await _context.Product.FindAsync(orderItem.ProductID);
            var order = await _context.Order.FindAsync(orderItem.OrderId);
            if (product != null)
            {
                product.Quantity -= orderItem.Quantity;
                _context.Product.Update(product);

                if(product.Quantity == 0)
                {
                    product.Isactive = false;
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound($"Product with ID {orderItem.ProductID} not found.");
            }
            if(order.ShipDate <= DateTime.Now)
            {
                order.Status = "Order Delivered";
                
            }
            return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderItemID }, orderItem);
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            try
            {
               

                var orderItem = await _context.OrderItems.FindAsync(id);
                var order = orderItem.OrderId;
                var ordered = await _context.Order.FindAsync(order);
             
                if (orderItem == null || ordered == null)
                {
                    return NotFound();
                }

                orderItem.Isactive = false;
                ordered.Isactive = false;

                var product = orderItem.ProductID;
                var productItem = await _context.Product.FindAsync(product);
                productItem.Quantity += orderItem.Quantity;



                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return NotFound();
        }

        private bool OrderItemExists(int id)
        {
            return (_context.OrderItems?.Any(e => e.OrderItemID == id)).GetValueOrDefault();
        }
    }
}
