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

        [Fact]
        public void SearchStock_EmptySearchTerm_ReturnsOriginalList()
        {
            // Arrange
            var service = new StockService();
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

        [Fact]
        public void SearchStock_TermMatchesProductName_ReturnsFilteredList()
        {
            // Arrange
            var service = new StockService();
            var source = new List<Stock>
            {
                new Stock { Product = new Product { Name = "Milk" } },
                new Stock { Product = new Product { Name = "Bread" } }
            };

            // Act
            var result = service.SearchStock(source, "milk");

            // Assert
            Assert.Single(result);
            Assert.Equal("Milk", result[0].Product.Name);
        }

        [Fact]
        public void SearchStock_TermMatchesProductCode_ReturnsFilteredList()
        {
            // Arrange
            var service = new StockService();
            var source = new List<Stock>
            {
                new Stock { Product = new Product { ProductCode = "P001", Name = "Milk" } },
                new Stock { Product = new Product { ProductCode = "P002", Name = "Bread" } }
            };

            // Act
            var result = service.SearchStock(source, "P002");

            // Assert
            Assert.Single(result);
            Assert.Equal("Bread", result[0].Product.Name);
        }

        [Fact]
        public void SearchStock_TermMatchesDescription_ReturnsFilteredList()
        {
            // Arrange
            var service = new StockService();
            var source = new List<Stock>
            {
                new Stock { Product = new Product { Name = "Milk", Description = "Fresh dairy product" } },
                new Stock { Product = new Product { Name = "Bread", Description = "Bakery product" } }
            };

            // Act
            var result = service.SearchStock(source, "dairy");

            // Assert
            Assert.Single(result);
            Assert.Equal("Milk", result[0].Product.Name);
        }

        [Fact]
        public void SearchStock_TermMatchesSupplierName_ReturnsFilteredList()
        {
            // Arrange
            var service = new StockService();
            var source = new List<Stock>
            {
                new Stock { Product = new Product { Name = "Milk", Supplier = new Supplier { Name = "Dairy Supplier" } } },
                new Stock { Product = new Product { Name = "Bread", Supplier = new Supplier { Name = "Bakery Supplier" } } }
            };

            // Act
            var result = service.SearchStock(source, "bakery");

            // Assert
            Assert.Single(result);
            Assert.Equal("Bread", result[0].Product.Name);
        }

        [Fact]
        public void SearchStock_SearchTermHasWhitespaceAndDifferentCase_ReturnsFilteredList()
        {
            // Arrange
            var service = new StockService();
            var source = new List<Stock>
            {
                new Stock { Product = new Product { Name = "Milk" } },
                new Stock { Product = new Product { Name = "Bread" } }
            };

            // Act
            var result = service.SearchStock(source, "  MILK  ");

            // Assert
            Assert.Single(result);
            Assert.Equal("Milk", result[0].Product.Name);
        }

        [Fact]
        public void SearchStock_TermDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            var service = new StockService();
            var source = new List<Stock>
            {
                new Stock { Product = new Product { Name = "Milk" } }
            };

            // Act
            var result = service.SearchStock(source, "Chocolate");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void SearchStock_SourceIsNull_ReturnsEmptyList()
        {
            // Arrange
            var service = new StockService();

            // Act
            var result = service.SearchStock(null, "any");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void SearchStock_SourceIsEmpty_ReturnsEmptyList()
        {
            // Arrange
            var service = new StockService();

            // Act
            var result = service.SearchStock(new List<Stock>(), "any");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void SearchStock_StockHasNullProduct_ReturnsEmptyList()
        {
            // Arrange
            var service = new StockService();
            var source = new List<Stock>
            {
                new Stock { Product = null }
            };

            // Act
            var result = service.SearchStock(source, "milk");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void SearchStock_NullSearchTerm_ReturnsOriginalList()
        {
            // Arrange
            var service = new StockService();
            var source = new List<Stock>
            {
                new Stock { Product = new Product { Name = "Milk" } }
            };

            // Act
            var result = service.SearchStock(source, null);

            // Assert
            Assert.Single(result);
            Assert.Equal(source, result);
        }
    }
}
