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
    public class AdressTest
    {
        [Test]
        public void GetAdress_ValidData_ReturnsListOfAdresses()
        {
            var adressData = new List<Adress>
            {
                new Adress
                {
                    AddressID = 1,
                    CustomerID = 101,
                    Street = "123 Main St",
                    City = "City",
                    State = "State",
                    ZipCode = "12345",
                    Country = "Country",
                    Isactive = true
                }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Adresses.AddRange(adressData);
            dbContext.SaveChanges();

            var controller = new AdressesController(dbContext);

            var result = controller.GetAdress() as List<Adress>;

            Assert.NotNull(result);
            Assert.AreEqual(adressData.Count, result.Count);
        }

       /* [Test]
        public void GetAdress_NoData_ThrowsException()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new AdressesController(dbContext);

            var ex = Assert.Throws<System.Exception>(() => controller.GetAdress());
            Assert.AreEqual("No Adress Available", ex.Message);
        }*/

        [Test]
        public void Post_ValidData_ReturnsCreatedResult()
        {
            var adress = new Adress
            {
                AddressID = 2,
                CustomerID = 102,
                Street = "456 Oak St",
                City = "City",
                State = "State",
                ZipCode = "54321",
                Country = "Country",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new AdressesController(dbContext);

            var result = controller.Post(adress) as CreatedResult;

            Assert.NotNull(result);
            Assert.AreEqual("Adress Added", "Adress Added");
        }

        [Test]
        public void Delete_ValidId_ReturnsOkResult()
        {
            var adressId = 3;
            var adress = new Adress
            {
                AddressID = adressId,
                CustomerID = 103,
                Street = "789 Pine St",
                City = "City",
                State = "State",
                ZipCode = "98765",
                Country = "Country",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Adresses.Add(adress);
            dbContext.SaveChanges();

            var controller = new AdressesController(dbContext);

            var result = controller.Delete(adressId) as OkResult;

            Assert.NotNull(result);

            Assert.False(adress.Isactive);
        }

        [Test]
        public void Delete_InvalidAdressId_ReturnsBadRequestResult()
        {
            var adressId = 4;
            var adress = new Adress
            {
                AddressID = adressId,
                CustomerID = 104,
                Street = "101 Elm St",
                City = "City",
                State = "State",
                ZipCode = "54321",
                Country = "Country",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            // Mock the DbContext to throw an exception when trying to delete the adress
            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.Adresses.Find(adressId)).Returns(adress);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated deletion error"));

            var controller = new AdressesController(mockDbContext.Object);

            // Act
            IActionResult result = null;
            try
            {
                result = controller.Delete(adressId);
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
        public async Task UpdateAdress_ValidData_ReturnsOkResult()
        {
            var adressId = 5;
            var adress = new Adress
            {
                AddressID = adressId,
                CustomerID = 105,
                Street = "555 Maple St",
                City = "City",
                State = "State",
                ZipCode = "12345",
                Country = "Country",
                Isactive = true
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Adresses.Add(adress);
            dbContext.SaveChanges();

            var controller = new AdressesController(dbContext);

            var result = await controller.UpdateAdress(adressId, 105, "123 Oak St", "New City", "New State", "54321", "New Country") as OkResult;

            Assert.NotNull(result);

            var updatedAdress = dbContext.Adresses.FirstOrDefault(p => p.AddressID == adressId);
            Assert.NotNull(updatedAdress);
            Assert.AreEqual(105, updatedAdress.CustomerID);
            Assert.AreEqual("123 Oak St", updatedAdress.Street);
            Assert.AreEqual("New City", updatedAdress.City);
            Assert.AreEqual("New State", updatedAdress.State);
            Assert.AreEqual("54321", updatedAdress.ZipCode);
            Assert.AreEqual("New Country", updatedAdress.Country);
        }

        [Test]
        public async Task UpdateAdress_InvalidId_ReturnsNotFoundResult()
        {
            var adressId = 6;

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new AdressesController(dbContext);

            var result = await controller.UpdateAdress(adressId, 106, "456 Pine St", "New City", "New State", "54321", "New Country") as NotFoundResult;

            Assert.NotNull(result);

            var nonExistingAdress = dbContext.Adresses.FirstOrDefault(p => p.AddressID == adressId);
            Assert.Null(nonExistingAdress);
        }

        [Test]
        public async Task GetAdressById_ValidId_ReturnsAdressesWithOkResult()
        {
            // Arrange
            var customerId = 19876;
            var adresses = new List<Adress>
            {
                new Adress { AddressID = 1656, CustomerID = customerId, Street = "123 Main St", City = "City1", State = "State1", ZipCode = "12345", Country = "Country1", Isactive = true },
                new Adress { AddressID = 27654345, CustomerID = customerId, Street = "456 Elm St", City = "City2", State = "State2", ZipCode = "67890", Country = "Country2", Isactive = true }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(dbContextOptions))
            {
                context.Adresses.AddRange(adresses);
                await context.SaveChangesAsync();

                var controller = new AdressesController(context);

                // Act
                var result = await controller.GetAdressById(customerId) as ActionResult<IEnumerable<Adress>>;

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<OkObjectResult>(result.Result);

                var okResult = result.Result as OkObjectResult;
                Assert.NotNull(okResult);

                var resultAdresses = okResult.Value as List<Adress>;
                Assert.NotNull(resultAdresses);
                Assert.AreEqual(adresses.Count, resultAdresses.Count);
                Assert.IsTrue(adresses.All(a => resultAdresses.Any(ra => ra.AddressID == a.AddressID)));
            }
        }

        [Test]
        public async Task GetAdressById_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var customerId = 1;
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new EcommerceDBContext(dbContextOptions))
            {
                var controller = new AdressesController(context);

                // Act
                var result = await controller.GetAdressById(customerId) as ActionResult<IEnumerable<Adress>>;

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<NotFoundResult>(result.Result);
            }
        }

       

    }
}
