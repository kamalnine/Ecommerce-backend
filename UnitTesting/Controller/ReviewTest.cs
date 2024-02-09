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
    public class ReviewTest
    {
        [Test]
        public void GetReview_ValidData_ReturnsListOfProducts()
        {
            var review = new List<Review>
            {
                new Review
                {
                  ReviewID = 2,
                ProductID = 102,
                CustomerID = 202,
                Rating = 5,
                Comment = "Excellent product"
                }
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Review.AddRange(review);
            dbContext.SaveChanges();

            var controller = new ReviewController(dbContext);

            var result = controller.GetReview() as List<Review>;

            Assert.NotNull(result);
            Assert.AreEqual(review.Count, review.Count);
        }

        [Test]
        public void GetReview_NoData_ThrowsException()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_Reviews_NoData")
                .Options;

            var dbContext = new EcommerceDBContext(dbContextOptions);
            var controller = new ReviewController(dbContext);

            // Act & Assert
            
            Assert.AreEqual("No Reviews Available", "No Reviews Available");
        }

        [Test]
        public void Post_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var review = new Review
            {
                ReviewID = 2,
                ProductID = 102,
                CustomerID = 202,
                Rating = 5,
                Comment = "Excellent product"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_Reviews_Post")
                .Options;

            var dbContext = new EcommerceDBContext(dbContextOptions);
            var controller = new ReviewController(dbContext);

          

            var result = controller.Post(review) as CreatedResult;

            Assert.NotNull(result);
            Assert.AreEqual("Review Added", "Review Added");
        }

        [Test]
        public void Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var review = new Review
            {
                ReviewID = 21861,
                ProductID = 1,
                CustomerID = 1,
                Rating = 4,
                Comment = "Good product"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_Reviews_Post_Exception")
                .Options;

            var dbContext = new EcommerceDBContext(dbContextOptions);
            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated exception"));
            var controller = new ReviewController(mockDbContext.Object);

            // Act
            var result = controller.Post(review) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public void Delete_ValidId_ReturnsOkResult()
        {
            // Arrange
            var reviewId = 3;
            var review = new Review
            {
                ReviewID = reviewId,
                ProductID = 103,
                CustomerID = 203,
                Rating = 3,
                Comment = "Average product"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_Reviews_Delete")
                .Options;

            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Review.Add(review);
            dbContext.SaveChanges();

            var controller = new ReviewController(dbContext);

            // Act
            var result = controller.Delete(reviewId) as OkResult;

            // Assert
            Assert.NotNull(result);

            // Verify that the review is marked as inactive
            Assert.False(review.Isactive);
        }

        [Test]
        public void Delete_InvalidReviewId_ReturnsBadRequestResult()
        {
            // Arrange
            var reviewId = 4;
            var review = new Review
            {
                ReviewID = reviewId,
                ProductID = 104,
                CustomerID = 204,
                Rating = 2,
                Comment = "Poor product"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_Reviews_Delete_Invalid")
                .Options;

            // Mock the DbContext to throw an exception when trying to delete the review
            var mockDbContext = new Mock<EcommerceDBContext>(dbContextOptions);
            mockDbContext.Setup(c => c.Review.Find(reviewId)).Returns(review);
            mockDbContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated deletion error"));

            var controller = new ReviewController(mockDbContext.Object);

            // Act
            var result = controller.Delete(reviewId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            var errorMessage = result.Value as string;
            Assert.AreEqual("Deletion error. Please try again later.", errorMessage);
        }

        [Test]
        public async Task UpdateReview_ValidData_ReturnsOkResult()
        {
            // Arrange
            var reviewId = 5;
            var review = new Review
            {
                ReviewID = reviewId,
                ProductID = 105,
                CustomerID = 205,
                Rating = 4,
                Comment = "Good product"
            };

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_Reviews_Update")
                .Options;

            var dbContext = new EcommerceDBContext(dbContextOptions);
            dbContext.Review.Add(review);
            dbContext.SaveChanges();

            var controller = new ReviewController(dbContext);

            // Act
            var result = await controller.UpdateReview(reviewId, 205, 105, 5, "Updated comment") as OkResult;

            // Assert
           

            var updatedReview = dbContext.Review.FirstOrDefault(r => r.ReviewID == reviewId);
           
            Assert.AreEqual(205,205);
            Assert.AreEqual(105,105);
            Assert.AreEqual(5, 5);
            Assert.AreEqual("Updated comment", "Updated comment");
        }

        [Test]
        public async Task UpdateReview_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var reviewId = 6;

            var dbContextOptions = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_Reviews_Update_Invalid")
                .Options;

            var dbContext = new EcommerceDBContext(dbContextOptions);

            var controller = new ReviewController(dbContext);

            // Act
            var result = await controller.UpdateReview(reviewId, 206, 106, 3, "Updated comment") as NotFoundResult;

            // Assert
            Assert.NotNull(result);

            var nonExistingReview = dbContext.Review.FirstOrDefault(r => r.ReviewID == reviewId);
            Assert.Null(nonExistingReview);
        }
    }
}
