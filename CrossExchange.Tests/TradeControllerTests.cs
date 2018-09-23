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
    public class TradeControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();

        private readonly TradeController _tradeController;

        public TradeControllerTests()
        {
            _tradeController = new TradeController(_shareRepositoryMock.Object, _tradeRepositoryMock.Object, _portfolioRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllTradings_ShouldRetrunBadRequest()
        {
            int portFolioid = 1;
            List<Trade> list = new List<Trade>();
            // Arrange
            _tradeRepositoryMock.Setup(x => x.GetAllTradings(It.IsAny<int>())).ReturnsAsync(list);

            // Act
            var result = await _tradeController.GetAllTradings(portFolioid);

            // Assert
            var returnResult = result as BadRequestResult;
            Assert.IsInstanceOf(typeof(BadRequestResult), returnResult);
        }

        [Test]
        public async Task GetAllTradings_ShouldRetrunList()
        {
            int portFolioid = 1;
            List<Trade> list = new List<Trade>()
            {
                new Trade()
                {
                    Id=1,
                    Action="BUY",
                    NoOfShares=50,
                    PortfolioId=1,
                    Price=90,
                    Symbol="REL"
                }
            };
            // Arrange
            _tradeRepositoryMock.Setup(x => x.GetAllTradings(It.IsAny<int>())).ReturnsAsync(list);

            // Act
            var result = await _tradeController.GetAllTradings(portFolioid);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Post_ShouldRetrunBadRequestWhenModelIncorrect()
        {
            TradeModel model = null;

            // Arrange

            // Act
            var result = await _tradeController.Post(model);

            // Assert
            var returnResult = result as BadRequestObjectResult;
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), returnResult);
        }

        [Test]
        public async Task Post_ShouldRetrunBadRequestWhenComapnyNotRegister()
        {
            TradeModel model = new TradeModel
            {
                Action = "BUY",
                NoOfShares = 50,
                Symbol = "REL",
                PortfolioId = 1
            };

            // Arrange
            HourlyShareRate share = null;
            // Arrange
            _shareRepositoryMock.Setup(x => x.GetLatestPrice(It.IsAny<string>())).ReturnsAsync(share);

            // Act
            var result = await _tradeController.Post(model);

            // Assert
            var returnResult = result as BadRequestResult;
            Assert.IsInstanceOf(typeof(BadRequestResult), returnResult);
        }

        [Test]
        public async Task Post_ShouldRetrunOK()
        {
            TradeModel model = new TradeModel
            {
                Action = "BUY",
                NoOfShares = 50,
                Symbol = "REL",
                PortfolioId = 1
            };

            // Arrange
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
            var result = await _tradeController.Post(model);

            // Assert
            Assert.NotNull(result);
            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

    }
}
