using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTests
{
    public class StockServiceTests
    {
        [Fact]
        public void GetStock_WhenCalled_ReturnsAllStock()
        {
            // Arrange
            var mockRepo = A.Fake<IStockRepository>();
            var expectedStock = new List<Stock>
            {
                new Stock { ProductId = 1, Quantity = 10 },
                new Stock { ProductId = 2, Quantity = 5 }
            };
            A.CallTo(() => mockRepo.GetAll()).Returns(expectedStock.AsQueryable());
            var service = new StockService(mockRepo);

            // Act
            var result = service.GetStock();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedStock, result);
        }
    }
}
