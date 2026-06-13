using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTests
{
    public class ImportServiceTests
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
    }
}
