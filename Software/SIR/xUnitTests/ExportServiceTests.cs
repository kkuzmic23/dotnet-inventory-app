using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTests
{
    public class ExportServiceTests
    {
        [Fact]
        public void GetProducts_WhenCalled_ReturnsAllProducts()
        {
            // Arrange
            var productRepo = A.Fake<IProductCRUDRepository>();
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Milk" },
                new Product { Id = 2, Name = "Bread" }
            };
            A.CallTo(() => productRepo.GetAll()).Returns(products.AsQueryable());

            var service = new ExportService(
                productRepo,
                A.Fake<IStockExportRepository>(),
                A.Fake<IStockExportHasProductRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                A.Fake<IStockRepository>());

            // Act
            var result = service.GetProducts();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(products, result);
        }

        [Fact]
        public void GetExports_WhenCalled_ReturnsExportsNewestFirst()
        {
            // Arrange
            var exportRepo = A.Fake<IStockExportRepository>();
            var firstExport = new StockExport { Id = 1, CreatedAt = new DateTime(2024, 1, 1) };
            var secondExport = new StockExport { Id = 2, CreatedAt = new DateTime(2024, 2, 1) };
            A.CallTo(() => exportRepo.GetAll()).Returns(new List<StockExport> { firstExport, secondExport }.AsQueryable());

            var service = new ExportService(
                A.Fake<IProductCRUDRepository>(),
                exportRepo,
                A.Fake<IStockExportHasProductRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                A.Fake<IStockRepository>());

            // Act
            var result = service.GetExports();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(secondExport, result[0]);
            Assert.Equal(firstExport, result[1]);
        }

        [Fact]
        public void Export_NullRequest_ReturnsError()
        {
            // Arrange
            var service = new ExportService(
                A.Fake<IProductCRUDRepository>(),
                A.Fake<IStockExportRepository>(),
                A.Fake<IStockExportHasProductRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                A.Fake<IStockRepository>());

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
            var service = new ExportService(
                A.Fake<IProductCRUDRepository>(),
                A.Fake<IStockExportRepository>(),
                A.Fake<IStockExportHasProductRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                A.Fake<IStockRepository>());

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
            var stockRepo = A.Fake<IStockRepository>();
            A.CallTo(() => stockRepo.GetAll()).Returns(new List<Stock>().AsQueryable());

            var service = new ExportService(
                A.Fake<IProductCRUDRepository>(),
                A.Fake<IStockExportRepository>(),
                A.Fake<IStockExportHasProductRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                stockRepo);

            var request = new ExportRequest
            {
                Items = new List<ExportItemRequest>
                {
                    new ExportItemRequest { ProductId = 0, Quantity = 1 }
                }
            };

            // Act
            var result = service.Export(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid product.", result.ErrorMessage);
        }

        [Fact]
        public void Export_InvalidQuantity_ReturnsError()
        {
            // Arrange
            var stockRepo = A.Fake<IStockRepository>();
            A.CallTo(() => stockRepo.GetAll()).Returns(new List<Stock>
            {
                new Stock { ProductId = 1, Quantity = 10 }
            }.AsQueryable());

            var service = new ExportService(
                A.Fake<IProductCRUDRepository>(),
                A.Fake<IStockExportRepository>(),
                A.Fake<IStockExportHasProductRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                stockRepo);

            var request = new ExportRequest
            {
                Items = new List<ExportItemRequest>
                {
                    new ExportItemRequest { ProductId = 1, Quantity = 0 }
                }
            };

            // Act
            var result = service.Export(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Enter a valid quantity.", result.ErrorMessage);
        }

        [Fact]
        public void Export_ProductNotInStock_ReturnsError()
        {
            // Arrange
            var stockRepo = A.Fake<IStockRepository>();
            A.CallTo(() => stockRepo.GetAll()).Returns(new List<Stock>().AsQueryable());

            var service = new ExportService(
                A.Fake<IProductCRUDRepository>(),
                A.Fake<IStockExportRepository>(),
                A.Fake<IStockExportHasProductRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                stockRepo);

            var request = new ExportRequest
            {
                Items = new List<ExportItemRequest>
                {
                    new ExportItemRequest { ProductId = 1, Quantity = 1 }
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
            var stockRepo = A.Fake<IStockRepository>();
            A.CallTo(() => stockRepo.GetAll()).Returns(new List<Stock>
            {
                new Stock { ProductId = 1, Quantity = 2 }
            }.AsQueryable());

            var service = new ExportService(
                A.Fake<IProductCRUDRepository>(),
                A.Fake<IStockExportRepository>(),
                A.Fake<IStockExportHasProductRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                stockRepo);

            var request = new ExportRequest
            {
                Items = new List<ExportItemRequest>
                {
                    new ExportItemRequest { ProductId = 1, Quantity = 3 }
                }
            };

            // Act
            var result = service.Export(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Not enough stock.", result.ErrorMessage);
        }

        [Fact]
        public void Export_ValidRequest_DecreasesStock()
        {
            // Arrange
            var exportRepo = A.Fake<IStockExportRepository>();
            var exportItemRepo = A.Fake<IStockExportHasProductRepository>();
            var transactionRepo = A.Fake<IInventoryTransactionRepository>();
            var stockRepo = A.Fake<IStockRepository>();

            var stock = new Stock { ProductId = 1, Quantity = 10 };
            A.CallTo(() => stockRepo.GetAll()).Returns(new List<Stock> { stock }.AsQueryable());

            A.CallTo(() => exportRepo.Add(A<StockExport>._, false))
                .Invokes((StockExport stockExport, bool saveChanges) => stockExport.Id = 5);

            var service = new ExportService(
                A.Fake<IProductCRUDRepository>(),
                exportRepo,
                exportItemRepo,
                transactionRepo,
                stockRepo);

            var request = new ExportRequest
            {
                Items = new List<ExportItemRequest>
                {
                    new ExportItemRequest { ProductId = 1, Quantity = 3 }
                }
            };

            // Act
            var result = service.Export(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(7, stock.Quantity);           
        }
    }
}
