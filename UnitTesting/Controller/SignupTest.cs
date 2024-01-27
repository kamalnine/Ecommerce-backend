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
    [TestFixture]
    public class SignupTest
    {

        private SignupController _signupController;
        private EcommerceDBContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "SignupDatabase")
                .Options;

            _context = new EcommerceDBContext(options);
            _signupController = new SignupController(_context);
        }
      
      

        [Test]
        public void GetSignup_ValidData_ReturnsListOfSignup()
        {
            var signupData = new List<Signup>
            {
                new Signup
                {
                    Signupid = 3,
                    Name = "Aditya Bhoyar",
                    Email = "aditya@gmail.com",
                    Password = "aditya123#",
                    Mobile = 7083525723,
                    Isactive = true,
                    ConfirmPassword = "aditya123#"
                }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Signup.AddRange(signupData);
            dbContext.SaveChanges();

            var controller = new SignupController(dbContext);

            var result = controller.GetSignup() as List<Signup>;

            Assert.NotNull(result);
            Assert.AreEqual(signupData.Count, result.Count);
        }

        [Test]
        public void GetSignup_NoData_ThrowsException()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new SignupController(dbContext);

            var ex = Assert.Throws<System.Exception>(() => controller.GetSignup());
            Assert.AreEqual("No Login Detail Available", ex.Message);
        }

        [Test]
        public void Post_ValidData_ReturnsCreatedResult()
        {
            var signup = new Signup
            {
                Signupid = 2,
                Name = "Kamal Sutte",
                Email = "kamal@gmail.com",
                Password = "kamal123#",
                Mobile = 7620317944,
                Isactive = true,
                ConfirmPassword = "kamal123#"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new SignupController(dbContext);

            var result = controller.Post(signup) as CreatedResult;

            Assert.NotNull(result);
            Assert.AreEqual("user Added", result.Value);
        }

        [Test]
        public void Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            var signup = new Signup
            {
                Signupid = 20383,
                Name = "vren sdhjkd",
                Mobile = 76203179449,
                Isactive = true,
                ConfirmPassword = "kamal123#"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);

            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated exception"));

            var controller = new SignupController(mockDbContext.Object);

            var result = controller.Post(signup) as BadRequestResult;

            Assert.NotNull(result);
        }

        [Test]
        public void Delete_ValidId_ReturnsOkResult()
        {
            var signupId = 4;
            var signup = new Signup
            {
                Signupid = signupId,
                Email = "kamal@gmail.com",
                Password = "kamal1272623#",
                Isactive = false,
                Name = "kamal",
                ConfirmPassword = "kamal123#"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Signup.Add(signup);
            dbContext.SaveChanges();

            var controller = new SignupController(dbContext);

            var result = controller.Delete(signupId) as OkResult;

            Assert.NotNull(result);

            Assert.False(signup.Isactive);
        }

        [Test]
        public void Delete_InvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var signupId = 4;
            var signup = new Signup
            {
                Signupid = signupId,
                Email = "kamal@gmail.com",
                Password = "kamal123#",
                Isactive = true,
                Name = "kamal",
                ConfirmPassword = "kamal123#"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            // Mock the DbContext to throw an exception when trying to delete the user
            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.Signup.Find(signupId)).Returns(signup);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated deletion error"));

            var controller = new SignupController(mockDbContext.Object);

            // Act
            IActionResult result = null;
            try
            {
                result = controller.Delete(signupId);
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


    }
}
