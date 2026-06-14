using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTests.IntegrationTests
{
    public class StockServiceIntegrationTests
    {
        [Fact]
        public void Constructor_WhenCalled_CreatesService()
        {
            // Act
            var service = new StockService();

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void GetStock_WhenCalled_ReturnsAllStock()
        {
            // Arrange
            var service = new StockService();
            var repo = new StockRepository();
            var expectedCount = repo.GetAll().Count();

            // Act
            var result = service.GetStock();

            // Assert
            Assert.Equal(expectedCount, result.Count);
        }
    }
}
