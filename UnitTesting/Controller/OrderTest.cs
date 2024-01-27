using Ecommerce.Controllers;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Controller
{
    public class OrderTest
    {
        public int OrderId { get; private set; }

        [Test]
        public void GetOrder_ValidData_ReturnsListOfProducts()
        {
            var orderData = new List<Order>
            {
                new Order
                {
                OrderID = 1,
                CustomerID = 101,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now.AddDays(3),
                Status = "Shipped",
                TotalAmount = 500,
                Isactive = true
                }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Order.AddRange(orderData);
            dbContext.SaveChanges();

            var controller = new OrderController(dbContext);

            var result = controller.GetOrder() as List<Order>;

            Assert.NotNull(result);
            Assert.AreEqual(orderData.Count, result.Count);
        }
        [Test]
        public void GetOrder_NoData_ThrowsException()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new OrderController(dbContext);

            var ex = Assert.Throws<System.Exception>(() => controller.GetOrder());
            Assert.AreEqual("No Orders Available", ex.Message);
        }
        [Test]
        public void Post_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var order = new Order
            {
                OrderID = 1,
                CustomerID = 101,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now.AddDays(2),
                Status = "Processing",
                TotalAmount = 800.0m,
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var dbContext = new EcommerceDBContext(dbContextOptions);
            var controller = new OrderController(dbContext);

            // Act
            var result = controller.Post(order) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(201, result.StatusCode); // Ensure it returns Created status code

            // Verify that the order is added to the database
            var addedOrder = dbContext.Order.FirstOrDefault(o => o.OrderID == order.OrderID);
            Assert.NotNull(addedOrder);
            Assert.AreEqual(order.CustomerID, addedOrder.CustomerID);
            Assert.AreEqual(order.OrderDate, addedOrder.OrderDate);
            Assert.AreEqual(order.ShipDate, addedOrder.ShipDate);
            Assert.AreEqual(order.Status, addedOrder.Status);
            Assert.AreEqual(order.TotalAmount, addedOrder.TotalAmount);
            Assert.AreEqual(order.Isactive, addedOrder.Isactive);
        }

        [Test]
        public void Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            
            var order = new Order
            {
                OrderID = 21861,
                CustomerID = 1,
        
                ShipDate = DateTime.Now.AddDays(2),
                Status = "Processing",
                TotalAmount = 800,
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated exception"));

            var controller = new OrderController(mockDbContext.Object);

            var result = controller.Post(order) as BadRequestResult;

            Assert.NotNull(result);
        }
        [Test]
        public void Delete_ValidId_ReturnsOkResult()
        {
            var orderId = 2;
            var order = new Order
            {
                OrderID = orderId,
                CustomerID = 101,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now.AddDays(2),
                Status = "Processing",
                TotalAmount = 800.0m,
                Isactive = true

            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Order.Add(order);
            dbContext.SaveChanges();

            var controller = new OrderController(dbContext);

            var result = controller.Delete(orderId) as OkResult;

            Assert.NotNull(result);

            Assert.False(order.Isactive);
        }
        [Test]
        public void Delete_InvalidProductId_ReturnsBadRequestResult()
        {
            var orderId = 2;
            var order = new Order
            {
                OrderID = orderId,
                CustomerID = 101,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now.AddDays(2),
                Status = "Processing",
                TotalAmount = 800.0m,
                Isactive = true

            };


            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            // Mock the DbContext to throw an exception when trying to delete the product
            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.Order.Find(OrderId)).Returns(order);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated deletion error"));

            var controller = new OrderController(mockDbContext.Object);

            // Act
            IActionResult result = null;
            try
            {
                result = controller.Delete(OrderId);
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
        public async Task UpdateOrder_ValidData_ReturnsOkResult()
        {
            var orderId = 2;
            var order = new Order
            {
                OrderID = orderId,
                CustomerID = 101,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now.AddDays(2),
                Status = "Processing",
                TotalAmount = 800.0m,
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Order.Add(order);
            dbContext.SaveChanges();

            var controller = new OrderController(dbContext);

            var result = await controller.UpdateOrder(orderId, 101, DateTime.Now, DateTime.Now.AddDays(2), "Delivered", 700.8m) as OkResult;

            Assert.NotNull(result);

            var updatedOrder = dbContext.Order.FirstOrDefault(p => p.OrderID == orderId);
            Assert.NotNull(updatedOrder);
            Assert.AreEqual(101, updatedOrder.CustomerID);

            // Use a tolerance range for DateTime comparisons
            Assert.That(updatedOrder.OrderDate, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            Assert.That(updatedOrder.ShipDate, Is.EqualTo(DateTime.Now.AddDays(2)).Within(TimeSpan.FromSeconds(1)));

            Assert.AreEqual("Delivered", updatedOrder.Status);
            Assert.AreEqual(700.8m, updatedOrder.TotalAmount);
        }
        [Test]
        public async Task UpdateOrder_InvalidId_ReturnsNotFoundResult()
        {
            var orderId = 2;

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new OrderController(dbContext);

            var result = await controller.UpdateOrder(orderId, 101, DateTime.Now, DateTime.Now.AddDays(2), "Delivered", 700.8m) as NotFoundResult;

            Assert.NotNull(result);

            var nonExistingProduct = dbContext.Order.FirstOrDefault(p => p.OrderID == OrderId);
            Assert.Null(nonExistingProduct);
        }


    }
}
