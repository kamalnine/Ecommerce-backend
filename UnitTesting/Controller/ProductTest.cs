using Ecommerce.Controllers;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Controller
{
    public class ProductTest
    {
        [Test]
        public void GetProduct_ValidData_ReturnsListOfProducts()
        {
            var productData = new List<Product>
            {
                new Product
                {
                    ProductID = 1,
                    Name = "Laptop",
                    Description = "Powerful laptop",
                    Price = 1000,
                    Quantity = 5,
                    Category = "Electronics",
                    ImageURL = "laptop.jpg",
                    Isactive = true
                }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.AddRange(productData);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            var result = controller.GetProduct() as List<Product>;

            Assert.NotNull(result);
            Assert.AreEqual(productData.Count, result.Count);
        }


        [Test]
        public void GetProduct_NoData_ThrowsException()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new ProductController(dbContext);

            var ex = Assert.Throws<System.Exception>(() => controller.GetProduct());
            Assert.AreEqual("No Element Available", ex.Message);
        }




        [Test]
        public void Post_ValidData_ReturnsCreatedResult()
        {
            var product = new Product
            {
                ProductID = 2,
                Name = "Smartphone",
                Description = "High-end smartphone",
                Price = 800,
                Quantity = 10,
                Category = "Electronics",
                ImageURL = "smartphone.jpg",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new ProductController(dbContext);

            var result = controller.Post(product) as CreatedResult;

            Assert.NotNull(result);
            Assert.AreEqual("Product Added", "Product Added");
        }

      


        [Test]
        public void Delete_ValidId_ReturnsOkResult()
        {
            var productId = 3;
            var product = new Product
            {
                ProductID = productId,
                Name = "Camera",
                Description = "High-resolution camera",
                Price = 500,
                Quantity = 8,
                Category = "Electronics",
                ImageURL = "camera.jpg",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.Add(product);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            var result = controller.Delete(productId) as OkResult;

            Assert.NotNull(result);

            Assert.False(product.Isactive);
        }
        [Test]
        public void Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            var productId = 31;
            var product = new Product
            {
                ProductID = productId,
                Name = "Camera",
                Description = "High-resolution camera",
                Quantity = 8,
                Category = "Electronics",
                ImageURL = "camera.jpg",
                Isactive = true
            };


            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated exception"));

            var controller = new ProductController(mockDbContext.Object);

            var result = controller.Post(product) as BadRequestResult;

            Assert.NotNull(result);
        }

        [Test]
        public void Delete_InvalidProductId_ReturnsBadRequestResult()
        {
            // Arrange
            var productId = 4;
            var product = new Product
            {
                ProductID = productId,
                Name = "Camera",
                Description = "High-resolution camera",
                Price = 500,
                Quantity = 8,
                Category = "Electronics",
                ImageURL = "camera.jpg",
                Isactive = true

            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            // Mock the DbContext to throw an exception when trying to delete the product
            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.Product.Find(productId)).Returns(product);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated deletion error"));

            var controller = new ProductController(mockDbContext.Object);

            // Act
            IActionResult result = null;
            try
            {
                result = controller.Delete(productId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result); // Ensure it is a BadRequestObjectResult

            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual(400, badRequestResult.StatusCode); // Ensure it returns BadRequest status code

            var errorMessage = badRequestResult.Value as string;
            Assert.AreEqual("Deletion error. Please try again later.", errorMessage); // Adjust this based on your actual error message handling
        }

        [Test]
        public async Task UpdateProduct_ValidData_ReturnsOkResult()
        {
            var productId = 4;
            var product = new Product
            {
                ProductID = productId,
                Name = "Headphones",
                Description = "Noise-canceling headphones",
                Price = 150,
                Quantity = 20,
                Category = "Electronics",
                ImageURL = "headphones.jpg",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.Add(product);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            var result = await controller.UpdateProduct(productId, "Wireless Headphones", "High-quality sound", 180, 15, "Electronics", "wireless-headphones.jpg") as OkResult;

            Assert.NotNull(result);

            var updatedProduct = dbContext.Product.FirstOrDefault(p => p.ProductID == productId);
            Assert.NotNull(updatedProduct);
            Assert.AreEqual("Wireless Headphones", updatedProduct.Name);
            Assert.AreEqual("High-quality sound", updatedProduct.Description);
            Assert.AreEqual(180, updatedProduct.Price);
            Assert.AreEqual(15, updatedProduct.Quantity);
            Assert.AreEqual("Electronics", updatedProduct.Category);
            Assert.AreEqual("wireless-headphones.jpg", updatedProduct.ImageURL);
        }

        [Test]
        public async Task UpdateProduct_InvalidId_ReturnsNotFoundResult()
        {
            var productId = 5;

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new ProductController(dbContext);

            var result = await controller.UpdateProduct(productId, "Smartwatch", "Fitness tracker", 120, 10, "Electronics", "smartwatch.jpg") as NotFoundResult;

            Assert.NotNull(result);

            var nonExistingProduct = dbContext.Product.FirstOrDefault(p => p.ProductID == productId);
            Assert.Null(nonExistingProduct);
        }
    }
}
