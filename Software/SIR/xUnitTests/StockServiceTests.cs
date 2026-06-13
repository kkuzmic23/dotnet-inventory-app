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

        [Fact]
        public void SearchStock_EmptySearchTerm_ReturnsOriginalList()
        {
            // Arrange
            var service = new StockService(A.Fake<IStockRepository>());
            var source = new List<Stock>
            {
                new Stock { Product = new Product { Name = "Milk" } }
            };

            // Act
            var result = service.SearchStock(source, "");

            // Assert
            Assert.Single(result);
            Assert.Equal(source, result);
        }
    }
}
