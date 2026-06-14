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
        private readonly List<int> insertedProducts = new List<int>();

        [Fact]
        public void AddProduct_ValidProduct_PersistsToDB()
        {
            ProductService service = new ProductService();

            var product = new Product
            {
                ProductCode = "P661",
                Name = "Milk",
                ReorderLevel = 5, 
                SupplierId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            var result = service.AddProduct(product);

            Assert.True(result);
            Assert.True(product.Id > 0);
            insertedProducts.Add(product.Id);
        }

        [Fact]
        public void AddProduct_InvalidProduct_DoesNotPersistToDB()
        {
            ProductService service = new ProductService();

            var result = service.AddProduct(null);

            Assert.False(result);
        }

        [Fact]
        public void GetProducts_ReturnsProductsFromDB()
        {
            ProductService service = new ProductService();

            var product = new Product
            {
                ProductCode = "P662",
                Name = "Coca-Cola",
                ReorderLevel = 3,
                SupplierId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            service.AddProduct(product);
            insertedProducts.Add(product.Id);

            var result = service.GetProducts();

            Assert.NotEmpty(result);
            Assert.Contains(result, p => p.Id == product.Id);
        }

        [Fact]
        public void UpdateProduct_ValidProduct_UpdatesInDB()
        {
            ProductService service = new ProductService();

            var product = new Product
            {
                ProductCode = "P663",
                Name = "Juice",
                ReorderLevel = 2,
                SupplierId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            service.AddProduct(product);
            insertedProducts.Add(product.Id);

            product.Name = "Orange Juice";
            var result = service.UpdateProduct(product);

            Assert.True(result);
            var updated = service.GetProducts().FirstOrDefault(p => p.Id == product.Id);
            Assert.Equal("Orange Juice", updated.Name);
        }

        [Fact]
        public void RemoveProduct_ValidProduct_DeletesFromDB()
        {
            ProductService service = new ProductService();

            var product = new Product
            {
                ProductCode = "P664",
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
            ProductRepository repo = new ProductRepository();
            ProductService service = new ProductService();

            var result = service.RemoveProduct(null);

            Assert.False(result);
        }

        [Fact]
        public void ToggleProductActiveStatus_ActiveProduct_TogglesOffInDB()
        {
            ProductService service = new ProductService();

            var product = new Product
            {
                ProductCode = "P665",
                Name = "Bread",
                ReorderLevel = 4,
                SupplierId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            service.AddProduct(product);
            insertedProducts.Add(product.Id);

            var result = service.ToggleProductActiveStatus(product);

            Assert.True(result);
            Assert.False(product.IsActive);
            var updated = service.GetProducts().FirstOrDefault(p => p.Id == product.Id);
            Assert.False(updated.IsActive);
        }

        [Fact]
        public void ToggleProductActiveStatus_InactiveProduct_TogglesOnInDB()
        {
            ProductService service = new ProductService();

            var product = new Product
            {
                ProductCode = "P666",
                Name = "Butter",
                ReorderLevel = 2,
                SupplierId = 1,
                IsActive = false,
                CreatedAt = DateTime.Now
            };

            service.AddProduct(product);
            insertedProducts.Add(product.Id);

            var result = service.ToggleProductActiveStatus(product);

            Assert.True(result);
            Assert.True(product.IsActive);
            var updated = service.GetProducts().FirstOrDefault(p => p.Id == product.Id);
            Assert.True(updated.IsActive);
        }

        public void Dispose()
        {
            ProductService service = new ProductService();

            foreach (var id in insertedProducts)
            {
                var order = service.GetProducts().FirstOrDefault(o => o.Id == id);
                if (order != null)
                {
                    service.RemoveProduct(order);
                }
            }
        }
    }
}
