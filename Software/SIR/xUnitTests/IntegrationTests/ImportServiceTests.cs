using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTests
{
    public class ImportServiceIntegrationTests
    {
        [Fact]
        public void GetImports_WhenCalled_ReturnsAllImports()
        {
            // Arrange
            var mockItemRepo = A.Fake<IOrderItemCRUDRepository>();
            var imports = new List<OrderHasProduct> { new OrderHasProduct { ProductId = 1 } };
            A.CallTo(() => mockItemRepo.GetAll()).Returns(imports.AsQueryable());
            
            var service = new ImportService(
                mockItemRepo, 
                A.Fake<IStockRepository>(), 
                A.Fake<IInventoryTransactionRepository>(), 
                A.Fake<IOrderRepository>());

            // Act
            var result = service.GetImports();

            // Assert
            Assert.Single(result);
            A.CallTo(() => mockItemRepo.GetAll());
        }

        [Fact]
        public void ApplyImport_NoDetails_ReturnsFalse()
        {
            // Arrange
            var mockItemRepo = A.Fake<IOrderItemCRUDRepository>();
            A.CallTo(() => mockItemRepo.GetAll()).Returns(new List<OrderHasProduct>().AsQueryable());

            var service = new ImportService(mockItemRepo, A.Fake<IStockRepository>(), A.Fake<IInventoryTransactionRepository>(), A.Fake<IOrderRepository>());

            // Act
            var result = service.ApplyImport(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetImportSummaries_WhenCalled_GroupsProductsBySupplier()
        {
            // Arrange
            var mockItemRepo = A.Fake<IOrderItemCRUDRepository>();
            var items = new List<OrderHasProduct>
            {
                new OrderHasProduct
                {
                    ProductId = 1,
                    Quantity = 2,
                    Product = new Product { Id = 1, Name = "Milk" },
                    Order = new Order
                    {
                        Supplier = new Supplier { Id = 1, Name = "Supplier A" },
                        Status = "Ordered"
                    }
                },
                new OrderHasProduct
                {
                    ProductId = 1,
                    Quantity = 3,
                    Product = new Product { Id = 1, Name = "Milk" },
                    Order = new Order
                    {
                        Supplier = new Supplier { Id = 1, Name = "Supplier A" },
                        Status = "Ordered"
                    }
                }
            };
            A.CallTo(() => mockItemRepo.GetAll()).Returns(items.AsQueryable());

            var service = new ImportService(
                mockItemRepo,
                A.Fake<IStockRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                A.Fake<IOrderRepository>());

            // Act
            var result = service.GetImportSummaries();

            // Assert
            Assert.Single(result);
            Assert.Equal("Supplier A", result[0].SupplierName);
            Assert.Single(result[0].Products);
            Assert.Equal("Milk", result[0].Products[0].ProductName);
            Assert.Equal(5, result[0].Products[0].TotalQuantity);
        }

        [Fact]
        public void GetImportSummaries_ImportedOrders_AreSkipped()
        {
            // Arrange
            var mockItemRepo = A.Fake<IOrderItemCRUDRepository>();
            var items = new List<OrderHasProduct>
            {
                new OrderHasProduct
                {
                    Product = new Product { Id = 1, Name = "Milk" },
                    Order = new Order
                    {
                        Supplier = new Supplier { Id = 1, Name = "Supplier A" },
                        Status = "Imported"
                    }
                }
            };
            A.CallTo(() => mockItemRepo.GetAll()).Returns(items.AsQueryable());

            var service = new ImportService(
                mockItemRepo,
                A.Fake<IStockRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                A.Fake<IOrderRepository>());

            // Act
            var result = service.GetImportSummaries();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetImportDetailsBySupplier_WhenCalled_ReturnsSupplierDetails()
        {
            // Arrange
            var mockItemRepo = A.Fake<IOrderItemCRUDRepository>();
            var items = new List<OrderHasProduct>
            {
                new OrderHasProduct
                {
                    OrderId = 10,
                    ProductId = 1,
                    Quantity = 4,
                    Product = new Product { Id = 1, ProductCode = "P001", Name = "Milk" },
                    Order = new Order
                    {
                        Id = 10,
                        SupplierId = 1,
                        Status = "Ordered"
                    }
                },
                new OrderHasProduct
                {
                    OrderId = 11,
                    ProductId = 2,
                    Quantity = 8,
                    Product = new Product { Id = 2, ProductCode = "P002", Name = "Bread" },
                    Order = new Order
                    {
                        Id = 11,
                        SupplierId = 2,
                        Status = "Ordered"
                    }
                }
            };
            A.CallTo(() => mockItemRepo.GetAll()).Returns(items.AsQueryable());

            var service = new ImportService(
                mockItemRepo,
                A.Fake<IStockRepository>(),
                A.Fake<IInventoryTransactionRepository>(),
                A.Fake<IOrderRepository>());

            // Act
            var result = service.GetImportDetailsBySupplier(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(10, result[0].OrderId);
            Assert.Equal("P001", result[0].ProductCode);
            Assert.Equal("Milk", result[0].ProductName);
            Assert.Equal(4, result[0].Quantity);
        }

        [Fact]
        public void ApplyImport_ExistingStock_IncreasesStock()
        {
            // Arrange
            var mockItemRepo = A.Fake<IOrderItemCRUDRepository>();
            var mockStockRepo = A.Fake<IStockRepository>();
            var mockTransactionRepo = A.Fake<IInventoryTransactionRepository>();
            var mockOrderRepo = A.Fake<IOrderRepository>();

            var existingStock = new Stock { ProductId = 1, Quantity = 10 };
            var items = new List<OrderHasProduct>
            {
                new OrderHasProduct
                {
                    OrderId = 5,
                    ProductId = 1,
                    Quantity = 4,
                    Product = new Product { Id = 1, ProductCode = "P001", Name = "Milk" },
                    Order = new Order
                    {
                        Id = 5,
                        SupplierId = 1,
                        Status = "Ordered"
                    }
                }
            };

            A.CallTo(() => mockItemRepo.GetAll()).Returns(items.AsQueryable());
            A.CallTo(() => mockStockRepo.GetByProductId(1)).Returns(existingStock);

            var service = new ImportService(
                mockItemRepo,
                mockStockRepo,
                mockTransactionRepo,
                mockOrderRepo);

            // Act
            var result = service.ApplyImport(1);

            // Assert
            Assert.True(result);
            Assert.Equal(14, existingStock.Quantity);
        }

        [Fact]
        public void ApplyImport_NewStock_AddsStock()
        {
            // Arrange
            var mockItemRepo = A.Fake<IOrderItemCRUDRepository>();
            var mockStockRepo = A.Fake<IStockRepository>();
            var items = new List<OrderHasProduct>
            {
                new OrderHasProduct
                {
                    OrderId = 5,
                    ProductId = 1,
                    Quantity = 4,
                    Product = new Product { Id = 1, ProductCode = "P001", Name = "Milk" },
                    Order = new Order
                    {
                        Id = 5,
                        SupplierId = 1,
                        Status = "Ordered"
                    }
                }
            };

            A.CallTo(() => mockItemRepo.GetAll()).Returns(items.AsQueryable());
            A.CallTo(() => mockStockRepo.GetByProductId(1)).Returns(null);

            var service = new ImportService(
                mockItemRepo,
                mockStockRepo,
                A.Fake<IInventoryTransactionRepository>(),
                A.Fake<IOrderRepository>());

            // Act
            var result = service.ApplyImport(1);

            // Assert
            Assert.True(result);
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
