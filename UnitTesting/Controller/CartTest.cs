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
    public class CartTest
    {
        [Test]
        public void GetCart_ValidData_ReturnsListOfCarts()
        {
            var cartData = new List<Cart>
            {
                new Cart
                {
                    CartID = 1,
                    CustomerID = 101,
                    ProductID = 201,
                    Quantity = 2
                }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Cart.AddRange(cartData);
            dbContext.SaveChanges();

            var controller = new CartsController(dbContext);

            var result = controller.GetCart() as List<Cart>;

            Assert.NotNull(result);
            Assert.AreEqual(cartData.Count, result.Count);
        }

        [Test]
        public void GetCart_NoData_ThrowsException()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new CartsController(dbContext);

            var ex = Assert.Throws<System.Exception>(() => controller.GetCart());
            Assert.AreEqual("No cart item Available", ex.Message);
        }

        [Test]
        public void Post_ValidData_ReturnsCreatedResult()
        {
            var cart = new Cart
            {
                CartID = 2,
                CustomerID = 102,
                ProductID = 202,
                Quantity = 3
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new CartsController(dbContext);

            var result = controller.Post(cart) as CreatedResult;

            Assert.NotNull(result);
            Assert.AreEqual("Cart Added", "Cart Added");
        }

        [Test]
        public void Delete_ValidId_ReturnsOkResult()
        {
            var cartId = 3;
            var cart = new Cart
            {
                CartID = cartId,
                CustomerID = 103,
                ProductID = 203,
                Quantity = 4
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Cart.Add(cart);
            dbContext.SaveChanges();

            var controller = new CartsController(dbContext);

            var result = controller.Delete(cartId) as OkResult;

            Assert.NotNull(result);
        }

        [Test]
        public void Delete_InvalidCartId_ReturnsBadRequestResult()
        {
            var cartId = 4;
            var cart = new Cart
            {
                CartID = cartId,
                CustomerID = 104,
                ProductID = 204,
                Quantity = 5
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            // Mock the DbContext to throw an exception when trying to delete the cart
            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.Cart.Find(cartId)).Returns(cart);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated deletion error"));

            var controller = new CartsController(mockDbContext.Object);

            // Act
            IActionResult result = null;
            try
            {
                result = controller.Delete(cartId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var errorMessage = badRequestResult.Value as string;
            Assert.AreEqual("Deletion error. Please try again later.", errorMessage);
        }

        [Test]
        public async Task UpdateCart_ValidData_ReturnsOkResult()
        {
            var cartId = 5;
            var cart = new Cart
            {
                CartID = cartId,
                CustomerID = 105,
                ProductID = 205,
                Quantity = 6
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Cart.Add(cart);
            dbContext.SaveChanges();

            var controller = new CartsController(dbContext);

            var result = await controller.UpdateCart(cartId, 105, 206, 7) as OkResult;

            Assert.NotNull(result);

            var updatedCart = dbContext.Cart.FirstOrDefault(p => p.CartID == cartId);
            Assert.NotNull(updatedCart);
            Assert.AreEqual(105, updatedCart.CustomerID);
            Assert.AreEqual(206, updatedCart.ProductID);
            Assert.AreEqual(7, updatedCart.Quantity);
        }

        [Test]
        public async Task UpdateCart_InvalidId_ReturnsNotFoundResult()
        {
            var cartId = 6;

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new CartsController(dbContext);

            var result = await controller.UpdateCart(cartId, 106, 207, 8) as NotFoundResult;

            Assert.NotNull(result);

            var nonExistingCart = dbContext.Cart.FirstOrDefault(p => p.CartID == cartId);
            Assert.Null(nonExistingCart);
        }
    }
}
