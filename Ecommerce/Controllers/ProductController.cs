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
          
          
            List<Product> products = _context.Product.ToList();
           
            return products;
        }
        [HttpGet("GetProductByCategory")]
        public List<Product> GetProductByCategory(string category)
        {
            var productBycategory = _context.Product.Where(p=>p.Category.ToLower() == category.ToLower()).ToList();
          
            
                return productBycategory;
           
          
            
        }
        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById(int id)
        {
           
                var product = _context.Product.Find(id);

                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                return Ok(product);
          
        }
     
        [HttpGet("GetProductsByPriceRange")]
        public IActionResult GetProductsByPriceRange(int minPrice, int maxPrice)
        {

                var products = _context.Product
                    .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                    .ToList();

                if (products.Count == 0)
                {
                    return NotFound("No products found within the specified price range.");
                }

                return Ok(products);
            
           
        }
        [HttpGet("GetProductImageById/{id}")]
        public IActionResult GetProductImageById(int id)
        {
           
                var product = _context.Product.Find(id);

                

                return Ok(product.ImageURL);
           
        }
        [HttpGet("Search")]
        public IActionResult Search(string keyword)
        {
           
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return BadRequest("Please provide a valid search keyword.");
                }

                var products = _context.Product.Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword)).ToList();

                if (products.Count == 0)
                {
                    return NotFound($"No products found containing the keyword '{keyword}'.");
                }

                return Ok(products);
            
            
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
            

            return Ok();

        }


    }
}
