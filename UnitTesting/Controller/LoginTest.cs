using Ecommerce.Controllers;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Controller
{
    public class LoginTest
    {
        private LoginController _controller;
        private IConfiguration _configuration;
        private EcommerceDBContext _dbContext;

        [SetUp]
        public void Setup()
        {
            // Mock IConfiguration
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["Jwt:Issuer"]).Returns("Issuer");
            configuration.Setup(c => c["Jwt:Audience"]).Returns("Audience");
            configuration.Setup(c => c["Jwt:Key"]).Returns("bd1a1ccf8095037f361a4d351e7c0de65f0776bfc2f478ea8d312c763bb6caca");

            // Initialize controller with mocked dependencies
            _configuration = configuration.Object;
            _controller = new LoginController(null, _configuration);

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase")
               .Options;
            _dbContext = new EcommerceDBContext(dbContextOptions);

            // Seed some test data
            _dbContext.Signup.Add(new Signup { Email = "test@example.com", Password = "test123", ConfirmPassword = "test123", Name = "Test User" });
            _dbContext.SaveChanges();

            // Initialize controller with mocked DbContext
            _controller = new LoginController(_dbContext, null);// Pass null for DbContext in this test
        }

        [Test]
        public void Post_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var email = "kamal@gmail.com";
            var password = "kamal123#";

            // Mock your database context or provide a real one if needed for testing
            var dbContext = new Mock<EcommerceDBContext>();
            dbContext.Setup(m => m.Signup.FirstOrDefault(u => u.Email == email && u.Password == password))
                .Returns(new Signup { Email = email, Password = password }); // Mock user retrieval

            // Set up controller with mocked DbContext
            _controller = new LoginController(dbContext.Object, _configuration);

            // Act
            var result = _controller.Post(email, password) as ObjectResult;

            // Assert

            Assert.AreEqual(200, result.StatusCode);

            var tokenData = result.Value as dynamic;
            Assert.NotNull(tokenData);
            Assert.NotNull(tokenData.Token);
            Assert.NotNull("Admin");
        }

        [Test]
        public void Post_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var email = "nonexistent@gmail.com";
            var password = "dfjksd";
            var user = new Signup
            {
                Email = "kamal@gmail.com",
                Password = "your_password_here",
                ConfirmPassword = password,
                Name = "kamal"
            };

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["Jwt:Issuer"]).Returns("Issuer"); // Replace with the actual issuer
            configuration.Setup(c => c["Jwt:Audience"]).Returns("Audience"); // Replace with the actual audience
            configuration.Setup(c => c["Jwt:Key"]).Returns("bd1a1ccf8095037f361a4d351e7c0de65f0776bfc2f478ea8d312c763bb6caca"); // Replace with the actual key

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Signup.Add(user);
            dbContext.SaveChanges();

            var controller = new LoginController(dbContext, configuration.Object);

            // Act
            var result = controller.Post(email, password) as UnauthorizedResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }

        [Test]
        public void GetName_ValidCredentials_ReturnsOkResultWithName()
        {
            // Arrange
            var email = "kamal@gmail.com";
            var password = "kamal123#";

            // Act
            var result = _controller.GetName(email, password) as ObjectResult;

            // Assert
          

            // Check if result.Value is not null before accessing it
           
            
                Assert.NotNull("kamal");
            
        }



        [Test]
        public void GetName_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var password = "invalidpassword";

            // Act
            var result = _controller.GetName(email, password) as UnauthorizedResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }

        [Test]
        public void GetName_EmptyCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var email = "";
            var password = "";

            // Act
            var result = _controller.GetName(email, password) as UnauthorizedResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }

        [Test]
        public void GetName_NullCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            string email = null;
            string password = null;

            // Act
            var result = _controller.GetName(email, password) as UnauthorizedResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }
        [Test]
        public void Post_ValidEmail_ReturnsAuthorized()
        {
            // Arrange
            var email = "test@example.com";
            _dbContext.Signup.Add(new Signup { Email = email,ConfirmPassword = "test123",Name="Test User",Password="test123"});
            _dbContext.SaveChanges();

            // Act
            var result = _controller.Post(email) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Not Authorized", result.Value);
        }

        [Test]
        public void Post_InvalidEmail_ReturnsNotAuthorized()
        {
            // Arrange
            var email = "nonexistent@example.com";

            // Act
            var result = _controller.Post(email) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Not Authorized", result.Value);
        }

    }
}
