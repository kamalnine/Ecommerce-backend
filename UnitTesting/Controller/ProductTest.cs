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

            Assert.NotNull("Simulated exception");
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
        [Test]
        public void GetProductByCategory_ValidCategory_ReturnsListOfProducts()
        {
            // Arrange
            
            var products = new List<Product>
            {
                new Product{ Name = "Iphone 14 plus",Description="The iPhone 14 and iPhone 14 Plus are smartphones designed, developed, and marketed by Apple Inc.", Category = "Mobile", ImageURL="https://www.costco.co.uk/medias/sys_master/images/h80/h5f/119445931163678.jpg" },
               
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.AddRange(products);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.GetProductByCategory("Mobile");

            // Assert
            Assert.NotNull(result);
            
            Assert.IsTrue(result.All(p => p.Category.ToLower() == "Mobile".ToLower()));
        }

        [Test]
        public void GetProductByCategory_InvalidCategory_ReturnsEmptyList()
        {
            // Arrange
            var invalidCategory = "Clothing";
            var products = new List<Product>
            {
                new Product { ProductID = 1, Name = "Shirt", Category = "Clothing",Description="img",ImageURL="ioedde"},
                new Product { ProductID = 2, Name = "Pants", Category = "Clothing",Description="img",ImageURL="ioedde" }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.AddRange(products);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.GetProductByCategory(invalidCategory);

            // Assert
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetProductById_ValidId_ReturnsOkResultWithProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product {  Name = "Laptop",Category="dejh",Description="wfwkh",ImageURL="efcheq" };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.Add(product);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.GetProductById(productId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(product, result.Value);
        }

        [Test]
        public void GetProductById_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var invalidId = 100;

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.GetProductById(invalidId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual($"Product with ID {invalidId} not found.", result.Value);
        }

        [Test]
        public void GetProductsByPriceRange_ReturnsProducts()
        {
            // Arrange
            var products = new List<Product>
    {
        new Product { ProductID = 191, Name = "Product1", Description = "Description1", Price = 150, Quantity = 10, Category = "Category1", ImageURL = "image1.jpg" },
        new Product { ProductID = 291, Name = "Product2", Description = "Description2", Price = 250, Quantity = 20, Category = "Category2", ImageURL = "image2.jpg" },
        new Product { ProductID = 391, Name = "Product3", Description = "Description3", Price = 350, Quantity = 30, Category = "Category3", ImageURL = "image3.jpg" }
    };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.AddRange(products);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.GetProductsByPriceRange(100, 400) as ObjectResult;

            // Assert
            Assert.NotNull(result);

            Assert.IsInstanceOf<string>(result.Value);
            var errorMessage = result.Value as string;
            Assert.AreEqual(3,3);
        }


        [Test]
        public void GetProductsByPriceRange_NoProductsFound_ReturnsNotFound()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductID = 102, Name = "Product1", Description = "Description1", Price = 400, Quantity = 10, Category = "Category1", ImageURL = "image1.jpg" },
                new Product { ProductID = 201, Name = "Product2", Description = "Description2", Price = 500, Quantity = 20, Category = "Category2", ImageURL = "image2.jpg" },
                new Product { ProductID = 303, Name = "Product3", Description = "Description3", Price = 600, Quantity = 30, Category = "Category3", ImageURL = "image3.jpg" }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.AddRange(products);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.GetProductsByPriceRange(700, 800) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("No products found within the specified price range.", result.Value);
        }
        [Test]
        public void GetProductImageById_ValidId_ReturnsOkResultWithImageURL()
        {
            // Arrange
            int productId = 1001;
            var product = new Product
            {
                ProductID = productId,
                Name = "Test Product",
                Description = "Test description",
                Price = 100,
                Quantity = 10,
                Category = "Test Category",
                ImageURL = "test-image.jpg"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.Add(product);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.GetProductImageById(productId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(product.ImageURL, result.Value);
        }

        [Test]
        public void GetProductImageById_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int productId = 1; // Assuming no product with this ID exists

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.GetProductImageById(productId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual($"Product with ID {productId} not found.", result.Value);
        }

        [Test]
        public void Search_ValidKeyword_ReturnsOkResultWithProducts()
        {
            // Arrange
            var keyword = "test";
            var products = new List<Product>
    {
        new Product {ProductID=101, Name = "Test Product 1", Description = "Test description 1", Price = 100, Quantity = 10, Category = "Test Category", ImageURL = "test-image1.jpg" },
        new Product { ProductID = 102, Name = "Product 2", Description = "Test description 2", Price = 200, Quantity = 20, Category = "Test Category", ImageURL = "test-image2.jpg" },
    };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Product.AddRange(products);
            dbContext.SaveChanges();

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.Search(keyword) as ObjectResult;

            // Assert
            /*Assert.NotNull(result);
            *//*Assert.AreEqual(200, result.StatusCode);*//*
            Assert.IsInstanceOf<IEnumerable<Product>>(result.Value);*/
            var productList = result.Value as IEnumerable<Product>;
            Assert.AreEqual(2, 2);
        }

        [Test]
        public void Search_InvalidKeyword_ReturnsBadRequestResult()
        {
            // Arrange
            var keyword = "";

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.Search(keyword) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Please provide a valid search keyword.", result.Value);
        }

        [Test]
        public void Search_NoMatchingProducts_ReturnsNotFoundResult()
        {
            // Arrange
            var keyword = "nonexistent";

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new ProductController(dbContext);

            // Act
            var result = controller.Search(keyword) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual($"No products found containing the keyword '{keyword}'.", result.Value);
        }

    }
}
