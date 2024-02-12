using NUnit.Framework;
using Ecommerce.Controllers;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Tests
{
    [TestFixture]
    public class CartControllerTests
    {
        private EcommerceDBContext _context;
        private CartController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new EcommerceDBContext(options);
            _controller = new CartController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task PostCart_WhenValidCart_ReturnsOkResult()
        {
            // Arrange
            var cart = new Cart
            {
                // Set all properties for a valid cart
                CustomerID = 1,
                ProductID = 1,
                Name = "Product Name",
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 2,
                Variant = "Product Variant",
                Category = "Product Category",
                ImageURL = "https://example.com/image.jpg",
                TotalPrice = 21.98m,
                Isactive = true
            };

            // Act
            var result = await _controller.PostCart(cart);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task DeleteCart_WhenValidId_ReturnsNoContentResult()
        {
            // Arrange
            var cart = new Cart
            {
                // Set all properties for a valid cart
                CustomerID = 1,
                ProductID = 1,
                Name = "Product Name",
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 2,
                Variant = "Product Variant",
                Category = "Product Category",
                ImageURL = "https://example.com/image.jpg",
                TotalPrice = 21.98m,
                Isactive = true
            };
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteCart(cart.CartID);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetCartByCustomer_WhenValidCustomerId_ReturnsListOfCart()
        {
            // Arrange
            var customerId = 1; // Provide a valid customer ID
            var cart = new Cart
            {
                // Set all properties for a valid cart
                CustomerID = customerId,
                ProductID = 1,
                Name = "Product Name",
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 2,
                Variant = "Product Variant",
                Category = "Product Category",
                ImageURL = "https://example.com/image.jpg",
                TotalPrice = 21.98m,
                Isactive = true
            };
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetCartByCustomer(customerId);

            // Assert
            Assert.IsInstanceOf<List<Cart>>(result.Value);
            Assert.AreEqual(1, result.Value.Count());
        }
    }
}
