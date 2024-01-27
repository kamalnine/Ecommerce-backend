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
        [Test]
        public void Post_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var email = "kamal@gmail.com";
            var password = "kamal123#";
            var name = "kamal";
            var user = new Signup
            {
               
                Email = email,
                Password = password,
                ConfirmPassword = password,
                Name = name

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
            var result = controller.Post(email, password) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var tokenData = result.Value as dynamic;
            Assert.NotNull(tokenData);
            Assert.IsNotNull(tokenData.Token);
            Assert.IsNotNull(tokenData.Role);
            Assert.AreEqual("Admin", tokenData.Role);
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
    }
}
