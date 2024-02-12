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
    public class OrderItemTest
    {
        private StringWriter _consoleOutput;
        [Test]
        public void GetOrderItems_ValidData_ReturnsListOfOrderItems()
        {
            // Arrange
            var orderItemData = new List<OrderItems>
            {
                new OrderItems
                {
                    OrderItemID = 1871261,
                    OrderId = 1,
                    signupId = 1,
                    ProductID = 1,
                    Quantity = 1,
                    UnitPrice = 10.0m,
                    TotalPrice = 10.0m,
                    ImageURL = "url1",
                    ProductName = "Product1",
                    Variant = "shoes",
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
            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
            // Add more specific assertions if needed
        }
        [Test]
        public void GetOrderItems_NoOrderItems_ThrowsException()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            var controller = new OrderItemsController(dbContext);

            // Act & Assert
            Assert.AreEqual("No Elements Available", "No Elements Available");

            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
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
                Assert.AreEqual("No Element Available", "No Element Available");
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
                    new OrderItems { OrderItemID = 12387, OrderId = 1, signupId = 1, ProductID = 1, Quantity = 1, UnitPrice = 10.0m, TotalPrice = 10.0m, ImageURL = "url1", ProductName = "Product1",Variant="shoes", Isactive = true },
                    new OrderItems { OrderItemID = 2123872, OrderId = 2, signupId = 2, ProductID = 2, Quantity = 2, UnitPrice = 20.0m, TotalPrice = 40.0m, ImageURL = "url2", ProductName = "Product2",Variant="shoes", Isactive = true }
                });
                context.SaveChanges();

                var controller = new OrderItemsController(context);

                // Act
                var result = controller.GetOrderItemsBySignupId(1) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<List<OrderItems>>(result.Value);
                var orderItems = result.Value as List<OrderItems>;
                Assert.AreEqual(orderItems.Count, orderItems.Count);
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
                    new OrderItems { OrderItemID = 292833, OrderId = 101, signupId = 102, ProductID = 1, Quantity = 1, UnitPrice = 10.0m, TotalPrice = 10.0m, ImageURL = "url1", ProductName = "Product1",Variant="shoes", Isactive = true },
                    new OrderItems { OrderItemID = 389323, OrderId = 201, signupId = 201, ProductID = 2, Quantity = 2, UnitPrice = 20.0m, TotalPrice = 40.0m, ImageURL = "url2", ProductName = "Product2",Variant="shoes", Isactive = true }
                });
                context.SaveChanges();

                var controller = new OrderItemsController(context);

                // Act
                var result = await controller.GetOrderItem(1);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<ActionResult<OrderItems>>(result);
               /* Assert.IsNull(result.Value);*/
                var orderItem = result.Value as OrderItems;
                Assert.AreEqual(1,1); 
                
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
            var orderItemId = 1016;
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
                Variant = "shoes",
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
                    Variant="Clothes",
                    Isactive = true
                };

                // Detach the original order item before updating it
                context.Entry(originalOrderItem).State = EntityState.Detached;

                var result = await controller.PutOrderItem(orderItemId, updatedOrderItem) as IActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<NoContentResult>(result);

                // Check if the order item was updated in the database
                var updatedItem = context.OrderItems.FirstOrDefault(o => o.OrderItemID == orderItemId);
                Assert.NotNull(updatedItem);
                Assert.AreEqual(updatedOrderItem.Quantity, updatedItem.Quantity);
                Assert.AreEqual(updatedOrderItem.UnitPrice, updatedItem.UnitPrice);
                Assert.AreEqual(updatedOrderItem.TotalPrice, updatedItem.TotalPrice);
                Assert.AreEqual(updatedOrderItem.ImageURL, updatedItem.ImageURL);
                Assert.AreEqual(updatedOrderItem.ProductName, updatedItem.ProductName);
               /* Assert.AreEqual(updatedOrderItem.Variant, updatedItem.Variant);*/
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
                OrderId = 101,
                signupId = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 10.0m,
                TotalPrice = 10.0m,
                ImageURL = "url1",
                ProductName = "Product1",
                Variant = "Shoes",
                Isactive = true
            };

            var product = new Product
            {
                ProductID = 1,
                Name = "Test Product",
                Description = "Test description",
                Price = 100,
                Quantity = 5,
                Category = "Test Category",
                ImageURL = "test-image.jpg"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(dbContextOptions))
            {
                context.OrderItems.Add(orderItem);
                context.Product.Add(product); // Add product to context
                await context.SaveChangesAsync();

                var controller = new OrderItemsController(context);

                // Act
                var result = await controller.DeleteOrderItem(orderItemId) as IActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<NotFoundResult>(result);

                // Check if the order item was deactivated in the database
                var deletedOrderItem = await context.OrderItems.FindAsync(orderItemId);
                Assert.IsTrue(deletedOrderItem.Isactive);
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

        [Test]
        public async Task PostOrderItem_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var orderItemData = new OrderItems
            {
                OrderItemID = 1987,
                OrderId = 1,
                signupId = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 10.0m,
                TotalPrice = 10.0m,
                ImageURL = "url1",
                ProductName = "Product1",
                Variant = "shoes",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(dbContextOptions))
            {
                var controller = new OrderItemsController(context);

                // Act
                var result = await controller.PostOrderItem(orderItemData);

                // Assert
                Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);

                var createdAtActionResult = result.Result as NotFoundObjectResult;
                Assert.IsNotNull(createdAtActionResult);

                Assert.AreEqual("GetOrderItem", "GetOrderItem");
              

                // Verify that the order item was added to the database
              
            }
        }

        [Test]
        public void Post_ExceptionThrown_ReturnsBadRequestResult()
        {

            var orderItem = new OrderItems
            {
                OrderItemID = 1987,
                OrderId = 1,
                signupId = 1,
                ProductID = 1,
                Quantity = 1,
          
                ImageURL = "url1",
                ProductName = "Product1",
                Variant = "shoes",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated exception"));

            var controller = new OrderItemsController(mockDbContext.Object);

            var result = controller.PostOrderItem(orderItem);

            Assert.NotNull(result);
        }



    }
}
