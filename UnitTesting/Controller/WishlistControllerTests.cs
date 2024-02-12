using Ecommerce.Controllers;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Tests
{
    [TestFixture]
    public class WishlistControllerTests
    {
        private WishlistController _wishlistController;
        private EcommerceDBContext _context;

        [SetUp]
        public void Setup()
        {
            // Set up a new in-memory database for testing
            var options = new DbContextOptionsBuilder<EcommerceDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new EcommerceDBContext(options);

            _wishlistController = new WishlistController(_context);
        }

        [Test]
        public async Task PostWishlist_ValidInput_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var wishlist = new Wishlist { WishlistID = 1, CustomerID = 1, ProductID = 1, Name = "Test Product", Description = "Test Description", Price = 10.99m, Quantity = 1, Category = "Test Category", ImageURL = "test.jpg",Isactive=true };

            // Act
            var result = await _wishlistController.PostWishlist(wishlist);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task DeleteWishlist_ExistingId_ReturnsNoContentResult()
        {
            // Arrange
            var wishlist = new Wishlist { WishlistID = 1, CustomerID = 1, ProductID = 1, Name = "Test Product", Description = "Test Description", Price = 10.99m, Quantity = 1, Category = "Test Category", ImageURL = "test.jpg",Isactive=true };
            _context.Wishlist.Add(wishlist);
            await _context.SaveChangesAsync();

            // Act
            var result = await _wishlistController.DeleteWishlist(1);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetWishlistByCustomer_ValidId_ReturnsListOfWishlist()
        {
            // Arrange
            var wishlist = new Wishlist { WishlistID = 1, CustomerID = 1, ProductID = 1, Name = "Test Product", Description = "Test Description", Price = 10.99m, Quantity = 1, Category = "Test Category", ImageURL = "test.jpg",Isactive=true };
            _context.Wishlist.Add(wishlist);
            await _context.SaveChangesAsync();

            // Act
            var result = await _wishlistController.GetWishlistByCustomer(1);


            var wishlistList = result.Value as List<Wishlist>;
            Assert.IsNotNull(wishlistList);
            Assert.AreEqual(1, wishlistList.Count);
            Assert.AreEqual(1, wishlistList.Count());
        }



        [TearDown]
        public void TearDown()
        {
            // Dispose the context after each test
            _context.Dispose();
        }
    }
}
