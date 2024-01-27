using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EcommerceDBContext _context;

        public ProductController(EcommerceDBContext context)
        {
            _context = context;
        }

        [HttpGet("GetProduct")]
        public List<Product> GetProduct()
        {
            if (_context.Product.ToList() == null)
            {
                throw new System.Exception("No Elements Available");
            }
            List<Product> products = _context.Product.ToList();
            if (products.Count == 0)
            {
                throw new Exception("No Element Available");
            }
            return products;
        }
        [HttpGet("GetProductByCategory")]
        public List<Product> GetProductByCategory(string category)
        {
            var productBycategory = _context.Product.Where(p=>p.Category.ToLower() == category.ToLower()).ToList();
            try
            {
                if (productBycategory.Count == 0)
                {
                    throw new System.Exception("No Products Available Of This Category");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return productBycategory;
            
        }
        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var product = _context.Product.Find(id);

                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetProductsByPriceLessThan29999")]
        public IActionResult GetProductsByPriceLessThan29999()
        {
            try
            {
                var products = _context.Product.Where(p => p.Price <= 29999).ToList();

                if (products.Count == 0)
                {
                    return NotFound("No products found with price less than 29999.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetProductsByPriceLessThan49999")]
        public IActionResult GetProductsByPriceLessThan49999()
        {
            try
            {
                var products = _context.Product.Where(p => p.Price <= 49999).ToList();

                if (products.Count == 0)
                {
                    return NotFound("No products found with price less than 49999.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetProductsByPriceLessThan69999")]
        public IActionResult GetProductsByPriceLessThan69999()
        {
            try
            {
                var products = _context.Product.Where(p => p.Price <= 69999).ToList();

                if (products.Count == 0)
                {
                    return NotFound("No products found with price less than 69999.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
       [HttpGet("GetProductsByPriceLessThan89999")]
        public IActionResult GetProductsByPriceLessThan89999()
        {
            try
            {
                var products = _context.Product.Where(p => p.Price <= 89999).ToList();

                if (products.Count == 0)
                {
                    return NotFound("No products found with price less than 89999.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                _context.Product.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Created("Product Added", "Product Added");
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Product product = _context.Product.Find(id);
                product.Isactive = false;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                 return BadRequest("Deletion error. Please try again later.");

            }
            return Ok();
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, string name, string description, int price, int quantity, string category, string imageurl)
        {
            try
            {
                var ent = await _context.Product.FindAsync(id);
                var existingProduct= _context.Product.FirstOrDefault(p => p.ProductID == id);

                if (existingProduct == null)
                {
                    return NotFound();
                }
               existingProduct.Name = name;
                existingProduct.Description = description;
                existingProduct.Price = price;
                existingProduct.Quantity = quantity;
                existingProduct.Category = category;
                existingProduct.ImageURL = imageurl;

                _context.Entry(existingProduct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
