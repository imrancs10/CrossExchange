using System;
using System.Threading.Tasks;
using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CrossExchange.Tests
{
    public class ShareControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly ShareController _shareController;

        public ShareControllerTests()
        {
            _shareController = new ShareController(_shareRepositoryMock.Object);
            //GetContextWithData();
        }

        private ExchangeContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<ExchangeContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ExchangeContext(options);

            var share =
              new HourlyShareRate { Id = 1, Rate = 90, Symbol = "REL", TimeStamp = DateTime.Now };
            dbContext.Add(share);
            dbContext.SaveChanges();

            return dbContext;
        }

        [Test]
        public async Task Post_ShouldInsertHourlySharePrice()
        {
            var hourRate = new HourlyShareRateModel
            {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };

            // Arrange

            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            Assert.NotNull(result);
            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_ShouldReturnBadRequestHourlySharePrice()
        {
            HourlyShareRateModel hourRate = null;

            // Arrange

            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            var returnResult = result as BadRequestObjectResult;
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), returnResult);
        }

        [Test]
        public async Task Get_ShouldReturnNotFound()
        {
            string symbol = "REL";
            List<HourlyShareRate> list = new List<HourlyShareRate>();
            // Arrange
            _shareRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(list);
            // Act
            var result = await _shareController.Get(symbol);

            // Assert
            var returnResult = result as NotFoundResult;
            Assert.IsInstanceOf(typeof(NotFoundResult), returnResult);
        }

        [Test]
        public async Task Get_ShouldReturnList()
        {
            string symbol = "REL";
            var list = new List<HourlyShareRate>()
            {
                new HourlyShareRate()
                {
                    Id = 1,
                    Rate = 90,
                    Symbol = "REL",
                    TimeStamp = DateTime.Now
                }
            };

            // Arrange
            _shareRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(list);

            // Act
            var result = await _shareController.Get(symbol);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetLatestPrice_ShouldReturnNotFound()
        {
            string symbol = "REL";
            // Arrange

            // Act
            var result = await _shareController.GetLatestPrice(symbol);

            // Assert
            var returnResult = result as NotFoundResult;
            Assert.IsInstanceOf(typeof(NotFoundResult), returnResult);
        }

        [Test]
        public async Task GetLatestPrice_ShouldReturnShareEntity()
        {
            string symbol = "REL";
            HourlyShareRate share = new HourlyShareRate()
            {
                Id = 1,
                Rate = 90,
                Symbol = "REL",
                TimeStamp = DateTime.Now
            };
            // Arrange
            _shareRepositoryMock.Setup(x => x.GetLatestPrice(It.IsAny<string>())).ReturnsAsync(share);
            // Act
            var result = await _shareController.GetLatestPrice(symbol);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

    }
}
