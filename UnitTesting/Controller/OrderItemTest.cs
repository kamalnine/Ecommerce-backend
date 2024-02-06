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
    public class OrderItemTest
    {
        [Test]
        public void GetOrderItems_ValidData_ReturnsListOfOrderItems()
        {
            // Arrange
            var orderItemData = new List<OrderItems>
            {
                new OrderItems
                {
                    OrderItemID = 1,
                    OrderId = 1,
                    signupId = 1,
                    ProductID = 1,
                    Quantity = 1,
                    UnitPrice = 10.0m,
                    TotalPrice = 10.0m,
                    ImageURL = "url1",
                    ProductName = "Product1",
                    Isactive = true
                }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.OrderItems.AddRange(orderItemData);
            dbContext.SaveChanges();

            var controller = new OrderItemsController(dbContext);

            // Act
            var result = controller.GetOrderItems() as List<OrderItems>;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(orderItemData.Count, result.Count);
            // Add more specific assertions if needed
        }
        [Test]
        public void GetOrderItems_Throws_Exception_When_No_Items_Available()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(options))
            {
                var controller = new OrderItemsController(context);

                // Act and Assert
                Assert.Throws<Exception>(() => controller.GetOrderItems());
            }
        }
        [Test]
        public void GetOrderItemsBySignupId_Returns_OkResult_With_Valid_Id()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(options))
            {
                context.OrderItems.AddRange(new List<OrderItems>
                {
                    new OrderItems { OrderItemID = 1, OrderId = 1, signupId = 1, ProductID = 1, Quantity = 1, UnitPrice = 10.0m, TotalPrice = 10.0m, ImageURL = "url1", ProductName = "Product1", Isactive = true },
                    new OrderItems { OrderItemID = 2, OrderId = 2, signupId = 2, ProductID = 2, Quantity = 2, UnitPrice = 20.0m, TotalPrice = 40.0m, ImageURL = "url2", ProductName = "Product2", Isactive = true }
                });
                context.SaveChanges();

                var controller = new OrderItemsController(context);

                // Act
                var result = controller.GetOrderItemsBySignupId(1) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<List<OrderItems>>(result.Value);
                var orderItems = result.Value as List<OrderItems>;
                Assert.AreEqual(1, orderItems.Count);
                // Add more specific assertions if needed
            }
        }

        [Test]
        public void GetOrderItemsBySignupId_Returns_NotFound_With_Invalid_Id()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(options))
            {
                var controller = new OrderItemsController(context);

                // Act
                var result = controller.GetOrderItemsBySignupId(999) as NotFoundResult;

                // Assert
                Assert.NotNull(result);
                // Add more specific assertions if needed
            }
        }

        [Test]
        public void GetOrderItemsBySignupId_Returns_InternalServerError_On_Exception()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(options))
            {
                // Simulate an exception by passing an invalid context
                var invalidContext = new EcommerceDBContext();

                var controller = new OrderItemsController(invalidContext);

                // Act
                var result = controller.GetOrderItemsBySignupId(1) as ObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.AreEqual(500, result.StatusCode);
                // Add more specific assertions if needed
            }
        }
        [Test]
        public async Task GetOrderItem_Returns_OrderItem_With_Valid_Id()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(options))
            {
                context.OrderItems.AddRange(new List<OrderItems>
                {
                    new OrderItems { OrderItemID = 1, OrderId = 1, signupId = 1, ProductID = 1, Quantity = 1, UnitPrice = 10.0m, TotalPrice = 10.0m, ImageURL = "url1", ProductName = "Product1", Isactive = true },
                    new OrderItems { OrderItemID = 2, OrderId = 2, signupId = 2, ProductID = 2, Quantity = 2, UnitPrice = 20.0m, TotalPrice = 40.0m, ImageURL = "url2", ProductName = "Product2", Isactive = true }
                });
                context.SaveChanges();

                var controller = new OrderItemsController(context);

                // Act
                var result = await controller.GetOrderItem(1);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<ActionResult<OrderItems>>(result);
                Assert.IsNotNull(result.Value);
                var orderItem = result.Value as OrderItems;
                Assert.AreEqual(1, orderItem.OrderItemID);
                // Add more specific assertions if needed
            }
        }

        [Test]
        public async Task GetOrderItem_Returns_NotFound_With_Invalid_Id()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(options))
            {
                var controller = new OrderItemsController(context);

                // Act
                var result = await controller.GetOrderItem(999);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<ActionResult<OrderItems>>(result);
                Assert.IsInstanceOf<NotFoundResult>(result.Result);
                // Add more specific assertions if needed
            }
        }
        [Test]
        public async Task UpdateOrderItem_ValidData_ReturnsOkResult()
        {
            // Arrange
            var orderItemId = 101;
            var originalOrderItem = new OrderItems
            {
                OrderItemID = orderItemId,
                OrderId = 1,
                signupId = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 10.0m,
                TotalPrice = 10.0m,
                ImageURL = "url1",
                ProductName = "Product1",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(dbContextOptions))
            {
                context.OrderItems.Add(originalOrderItem);
                await context.SaveChangesAsync();

                var controller = new OrderItemsController(context);

                // Act
                var updatedOrderItem = new OrderItems
                {
                    OrderItemID = orderItemId,
                    OrderId = 1,
                    signupId = 1,
                    ProductID = 1,
                    Quantity = 2,
                    UnitPrice = 15.0m,
                    TotalPrice = 30.0m,
                    ImageURL = "updated-url",
                    ProductName = "Updated Product",
                    Isactive = true
                };

                // Detach the original order item before updating it
                context.Entry(originalOrderItem).State = EntityState.Detached;

                var result = await controller.PutOrderItem(orderItemId, updatedOrderItem) as IActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<OkResult>(result);

                // Check if the order item was updated in the database
                var updatedItem = context.OrderItems.FirstOrDefault(o => o.OrderItemID == orderItemId);
                Assert.NotNull(updatedItem);
                Assert.AreEqual(updatedOrderItem.Quantity, updatedItem.Quantity);
                Assert.AreEqual(updatedOrderItem.UnitPrice, updatedItem.UnitPrice);
                Assert.AreEqual(updatedOrderItem.TotalPrice, updatedItem.TotalPrice);
                Assert.AreEqual(updatedOrderItem.ImageURL, updatedItem.ImageURL);
                Assert.AreEqual(updatedOrderItem.ProductName, updatedItem.ProductName);
            }
        }

        [Test]
        public async Task PutOrderItem_Returns_BadRequest_With_Invalid_Id()
        {
            // Arrange
            var invalidId = 999;
            var orderItem = new OrderItems { OrderItemID = 1, OrderId = 1, signupId = 1, ProductID = 1, Quantity = 1, UnitPrice = 10.0m, TotalPrice = 10.0m, ImageURL = "url1", ProductName = "Product1", Isactive = true };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var mockContext = new Mock<EcommerceDBContext>(dbContextOptions);

            var controller = new OrderItemsController(mockContext.Object);

            // Act
            var result = await controller.PutOrderItem(invalidId, orderItem) as IActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task DeleteOrderItem_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var orderItemId = 1;
            var orderItem = new OrderItems
            {
                OrderItemID = orderItemId,
                OrderId = 1,
                signupId = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 10.0m,
                TotalPrice = 10.0m,
                ImageURL = "url1",
                ProductName = "Product1",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(dbContextOptions))
            {
                context.OrderItems.Add(orderItem);
                await context.SaveChangesAsync();

                var controller = new OrderItemsController(context);

                // Act
                var result = await controller.DeleteOrderItem(orderItemId) as IActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<NoContentResult>(result);

                // Check if the order item was deactivated in the database
                var deletedOrderItem = await context.OrderItems.FindAsync(orderItemId);
                Assert.IsFalse(deletedOrderItem.Isactive);
            }
        }

        [Test]
        public async Task DeleteOrderItem_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(dbContextOptions))
            {
                var controller = new OrderItemsController(context);

                // Act
                var result = await controller.DeleteOrderItem(999) as IActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<NotFoundResult>(result);
            }
        }


    }
}
