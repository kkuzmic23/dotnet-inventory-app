using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTests.IntegrationTests
{
    public class ProductIntegrationTests : IDisposable
    {
        private readonly ProductRepository repo = new ProductRepository();
        private readonly ProductService service = new ProductService();
        private readonly List<Product> insertedProducts = new List<Product>();

        [Fact]
        public void AddProduct_ValidProduct_PersistsToDB()
        {
            var product = new Product
            {
                ProductCode = "P001",
                Name = "Milk",
                ReorderLevel = 5, 
                SupplierId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            var result = service.AddProduct(product);

            Assert.True(result);
            Assert.True(product.Id > 0);
            insertedProducts.Add(product);
        }

        [Fact]
        public void AddProduct_InvalidProduct_DoesNotPersistToDB()
        {
            var result = service.AddProduct(null);

            Assert.False(result);
        }

        [Fact]
        public void GetProducts_ReturnsProductsFromDB()
        {
            var product = new Product
            {
                ProductCode = "P002",
                Name = "Coca-Cola",
                ReorderLevel = 3,
                SupplierId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            service.AddProduct(product);
            insertedProducts.Add(product);

            var result = service.GetProducts();

            Assert.NotEmpty(result);
            Assert.Contains(result, p => p.Id == product.Id);
        }

        [Fact]
        public void UpdateProduct_ValidProduct_UpdatesInDB()
        {
            var product = new Product
            {
                ProductCode = "P003",
                Name = "Juice",
                ReorderLevel = 2,
                SupplierId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            service.AddProduct(product);
            insertedProducts.Add(product);

            product.Name = "Orange Juice";
            var result = service.UpdateProduct(product);

            Assert.True(result);
            var updated = service.GetProducts().FirstOrDefault(p => p.Id == product.Id);
            Assert.Equal("Orange Juice", updated.Name);
        }

        [Fact]
        public void RemoveProduct_ValidProduct_DeletesFromDB()
        {
            var product = new Product
            {
                ProductCode = "P004",
                Name = "Water",
                ReorderLevel = 1,
                SupplierId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            service.AddProduct(product);

            var result = service.RemoveProduct(product);

            Assert.True(result);
            var found = service.GetProducts().FirstOrDefault(p => p.Id == product.Id);
            Assert.Null(found);
        }

        [Fact]
        public void RemoveProduct_NullProduct_ReturnsFalse()
        {
            var result = service.RemoveProduct(null);

            Assert.False(result);
        }

        [Fact]
        public void ToggleProductActiveStatus_ActiveProduct_TogglesOffInDB()
        {
            var product = new Product
            {
                ProductCode = "P005",
                Name = "Bread",
                ReorderLevel = 4,
                SupplierId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            service.AddProduct(product);
            insertedProducts.Add(product);

            var result = service.ToggleProductActiveStatus(product);

            Assert.True(result);
            Assert.False(product.IsActive);
            var updated = service.GetProducts().FirstOrDefault(p => p.Id == product.Id);
            Assert.False(updated.IsActive);
        }

        [Fact]
        public void ToggleProductActiveStatus_InactiveProduct_TogglesOnInDB()
        {
            var product = new Product
            {
                ProductCode = "P006",
                Name = "Butter",
                ReorderLevel = 2,
                SupplierId = 1,
                IsActive = false,
                CreatedAt = DateTime.Now
            };

            service.AddProduct(product);
            insertedProducts.Add(product);

            var result = service.ToggleProductActiveStatus(product);

            Assert.True(result);
            Assert.True(product.IsActive);
            var updated = service.GetProducts().FirstOrDefault(p => p.Id == product.Id);
            Assert.True(updated.IsActive);
        }

        public void Dispose()
        {
            foreach (var product in insertedProducts)
            {
                repo.Remove(product);
            }
            repo.Dispose();
        }
    }
}
