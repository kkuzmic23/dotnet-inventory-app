using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTests.IntegrationTests
{
    public class ImportServiceIntegrationTests
    {
        [Fact]
        public void GetImports_WhenCalled_ReturnsAllImports()
        {
            // Arrange
            var service = new ImportService();
            var repo = new OrderHasProductRepository();
            var expectedCount = repo.GetAll().Count();

            // Act
            var result = service.GetImports();

            // Assert
            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public void ApplyImport_NoDetails_ReturnsFalse()
        {
            // Arrange
            var service = new ImportService();

            // Act
            // Using a very unlikely supplier ID
            var result = service.ApplyImport(-999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetImportSummaries_WhenCalled_ReturnsSummariesFromDb()
        {
            // Arrange
            var service = new ImportService();
            var repo = new OrderHasProductRepository();
            var items = repo.GetAll().ToList();
            var expectedSupplierCount = items
                .Where(x => x.Order?.Supplier != null && x.Product != null && x.Order.Status != "Imported")
                .GroupBy(x => x.Order.Supplier.Id)
                .Count();

            // Act
            var result = service.GetImportSummaries();

            // Assert
            Assert.Equal(expectedSupplierCount, result.Count);
        }

        [Fact]
        public void GetImportDetailsBySupplier_WhenCalled_ReturnsSupplierDetails()
        {
            // Arrange
            var service = new ImportService();
            var repo = new OrderHasProductRepository();
            var firstItem = repo.GetAll().FirstOrDefault(x => x.Order.Status != "Imported");

            if (firstItem != null)
            {
                // Act
                var result = service.GetImportDetailsBySupplier(firstItem.Order.SupplierId);

                // Assert
                Assert.NotEmpty(result);
                Assert.All(result, item => Assert.Equal(firstItem.Order.SupplierId, repo.GetAll().First(x => x.OrderId == item.OrderId).Order.SupplierId));
            }
        }
    }
}
