using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTests.IntegrationTests
{
    public class ExportServiceIntegrationTests
    {
        [Fact]
        public void GetProducts_WhenCalled_ReturnsAllProducts()
        {
            // Arrange
            var service = new ExportService();
            var repo = new ProductRepository();
            var expectedCount = repo.GetAll().Count();

            // Act
            var result = service.GetProducts();

            // Assert            
            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public void GetExports_WhenCalled_ReturnsExportsNewestFirst()
        {
            // Arrange
            var service = new ExportService();
            var repo = new StockExportRepository();
            var expectedCount = repo.GetAll().Count();

            // Act
            var result = service.GetExports();

            // Assert
            Assert.Equal(expectedCount, result.Count);
            if (result.Count >= 2)
            {
                Assert.True(result[0].CreatedAt >= result[1].CreatedAt);
            }
        }

        [Fact]
        public void Export_NullRequest_ReturnsError()
        {
            // Arrange
            var service = new ExportService();

            // Act
            var result = service.Export(null);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Add at least one item to export.", result.ErrorMessage);
        }

        [Fact]
        public void Export_EmptyItems_ReturnsError()
        {
            // Arrange
            var service = new ExportService();

            // Act
            var result = service.Export(new ExportRequest());

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Add at least one item to export.", result.ErrorMessage);
        }

        [Fact]
        public void Export_InvalidProduct_ReturnsError()
        {
            // Arrange
            var service = new ExportService();

            var request = new ExportRequest
            {
                Items = new List<ExportItemRequest>
                {
                    new ExportItemRequest { ProductId = -999, Quantity = 1 }
                }
            };

            // Act
            var result = service.Export(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Product not in stock.", result.ErrorMessage);
        }

        [Fact]
        public void Export_InvalidQuantity_ReturnsError()
        {
            // Arrange
            var service = new ExportService();
            var stockRepo = new StockRepository();
            var firstInStock = stockRepo.GetAll().FirstOrDefault(s => s.Quantity > 0);

            if (firstInStock != null)
            {
                var request = new ExportRequest
                {
                    Items = new List<ExportItemRequest>
                    {
                        new ExportItemRequest { ProductId = firstInStock.ProductId, Quantity = 0 }
                    }
                };

                // Act
                var result = service.Export(request);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Enter a valid quantity.", result.ErrorMessage);
            }
        }

        [Fact]
        public void Export_ProductNotInStock_ReturnsError()
        {
            // Arrange
            var service = new ExportService();

            var request = new ExportRequest
            {
                Items = new List<ExportItemRequest>
                {
                    new ExportItemRequest { ProductId = -999, Quantity = 1 }
                }
            };

            // Act
            var result = service.Export(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Product not in stock.", result.ErrorMessage);
        }

        [Fact]
        public void Export_NotEnoughStock_ReturnsError()
        {
            // Arrange
            var service = new ExportService();
            var stockRepo = new StockRepository();
            var firstInStock = stockRepo.GetAll().FirstOrDefault(s => s.Quantity > 0);

            if (firstInStock != null)
            {
                var request = new ExportRequest
                {
                    Items = new List<ExportItemRequest>
                    {
                        new ExportItemRequest { ProductId = firstInStock.ProductId, Quantity = firstInStock.Quantity + 100 }
                    }
                };

                // Act
                var result = service.Export(request);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Not enough stock.", result.ErrorMessage);
            }
        }
    }
}
