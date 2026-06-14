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

        [Fact]
        public void FilterSummaries_SupplierFilter_ReturnsMatchingSuppliers()
        {
            // Arrange
            var service = new ImportService();
            var source = new List<SupplierImportSummary>
            {
                new SupplierImportSummary { SupplierName = "Supplier A", Products = new List<ProductImportSummary> { new ProductImportSummary { ProductName = "Product 1", TotalQuantity = 10 } } },
                new SupplierImportSummary { SupplierName = "Other B", Products = new List<ProductImportSummary> { new ProductImportSummary { ProductName = "Product 2", TotalQuantity = 5 } } }
            };

            // Act
            var result = service.FilterSummaries(source, "Supplier", null, null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Supplier A", result[0].SupplierName);
        }

        [Fact]
        public void FilterSummaries_ProductFilter_ReturnsMatchingProducts()
        {
            // Arrange
            var service = new ImportService();
            var source = new List<SupplierImportSummary>
            {
                new SupplierImportSummary
                {
                    SupplierName = "Supplier A",
                    Products = new List<ProductImportSummary>
                    {
                        new ProductImportSummary { ProductName = "Milk", TotalQuantity = 10 },
                        new ProductImportSummary { ProductName = "Bread", TotalQuantity = 5 }
                    }
                }
            };

            // Act
            var result = service.FilterSummaries(source, null, "Milk", null, null);

            // Assert
            Assert.Single(result);
            Assert.Single(result[0].Products);
            Assert.Equal("Milk", result[0].Products[0].ProductName);
        }

        [Fact]
        public void FilterSummaries_AmountFilter_ReturnsProductsInAmountRange()
        {
            // Arrange
            var service = new ImportService();
            var source = new List<SupplierImportSummary>
            {
                new SupplierImportSummary
                {
                    SupplierName = "Supplier A",
                    Products = new List<ProductImportSummary>
                    {
                        new ProductImportSummary { ProductName = "Milk", TotalQuantity = 10 },
                        new ProductImportSummary { ProductName = "Bread", TotalQuantity = 3 }
                    }
                }
            };

            // Act
            var result = service.FilterSummaries(source, null, null, 5, 12);

            // Assert
            Assert.Single(result);
            Assert.Single(result[0].Products);
            Assert.Equal("Milk", result[0].Products[0].ProductName);
        }

        [Fact]
        public void FilterSummaries_NullSource_ReturnsEmptyList()
        {
            // Arrange
            var service = new ImportService();

            // Act
            var result = service.FilterSummaries(null, "any", null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
