using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using FakeItEasy;
using System.Collections.Generic;
using Xunit;

namespace xUnitTests
{
    public class ProductTests
    {
        [Fact]
        public void ValidateProduct_ValidProduct_ReturnsSuccess()
        {
            ProductService service = new ProductService();

            Product product = new Product
            {
                ProductCode = "P0001",
                Name = "Product name",
                ReorderLevel = 4
            };

            ServiceResult result = service.ValidateProduct(product);

            Assert.True(result.IsSuccessful);
        }

        [Theory]
        [InlineData(null, "Product name")]
        [InlineData("", "Product name")]
        [InlineData("   ", "Product name")]
        [InlineData("P001", null)]
        [InlineData("P001", "")]
        [InlineData("P001", "   ")]
        public void ValidateProduct_MissingRequiredFields_ReturnsFailure(string productCode, string name)
        {
            ProductService service = new ProductService();

            Product product = new Product
            {
                ProductCode = productCode,
                Name = name,
                ReorderLevel = 5
            };

            ServiceResult result = service.ValidateProduct(product);

            Assert.False(result.IsSuccessful);
            Assert.Equal("Name and product code are required", result.ErrorMessage);
        }

        [Fact]
        public void ValidateProduct_NullProduct_ReturnsFailure()
        {
            ProductService service = new ProductService();

            ServiceResult result = service.ValidateProduct(null);

            Assert.False(result.IsSuccessful);
            Assert.Equal("Product is required", result.ErrorMessage);
        }

        [Fact]
        public void ValidateProduct_NegativeReorderLevel_ReturnsFailure()
        {
            ProductService service = new ProductService();

            Product product = new Product
            {
                ProductCode = "P0001",
                Name = "Product name",
                ReorderLevel = -2
            };

            ServiceResult result = service.ValidateProduct(product);

            Assert.False(result.IsSuccessful);
            Assert.Equal("Reorder level must be positive number", result.ErrorMessage);
        }

        [Fact]

        public void FilterProduct_EmptyProductList_ReturnsEmptyList()
        {
            ProductService service = new ProductService();

            List<Product> products = service.FilterProducts(null, 5, "milk");

            Assert.Empty(products);
        }

        [Fact]

        public void FilterProduct_ContainsSupplierId_ReturnsListFilteredBySupplier()
        {
            ProductService service = new ProductService();

            int targetId = 3;

            List<Product> products = new List<Product>
            {
                new Product{ ProductCode = "P001", Name = "Milk", Description = "Milk description", SupplierId = 1, ReorderLevel = 5, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P002", Name = "Coca-Cola", Description = "Coca-Cola description", SupplierId = 2, ReorderLevel = 2, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P003", Name = "Fanta", Description = "Fanta description", SupplierId = 3, ReorderLevel = 7, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P004", Name = "Water", Description = "Water description", SupplierId = 4, ReorderLevel = 8, IsActive = true, CreatedAt = new System.DateTime()},
            };

            List<Product> filteredProducts = service.FilterProducts(products, targetId, null);

            Assert.Equal(filteredProducts[0], products.Find(p => p.SupplierId == targetId));
        }

        [Fact]
        public void FilterProduct_ContainsSearchPhrase_ReturnsListFilteredBySearchPhrase()
        {
            ProductService service = new ProductService();

            string searchPhrase = "Milk";

            List<Product> products = new List<Product>
            {
                new Product{ ProductCode = "P001", Name = "Milk", Description = "Milk description", SupplierId = 1, ReorderLevel = 5, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P002", Name = "Coca-Cola", Description = "Coca-Cola description", SupplierId = 2, ReorderLevel = 2, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P003", Name = "Fanta", Description = "Fanta description", SupplierId = 3, ReorderLevel = 7, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P004", Name = "Water", Description = "Water description", SupplierId = 4, ReorderLevel = 8, IsActive = true, CreatedAt = new System.DateTime()},
            };

            List<Product> filteredProducts = service.FilterProducts(products, 0, searchPhrase);

            Assert.Equal(filteredProducts[0], products.Find(p => p.Name == "Milk"));
        }

        [Fact]
        public void FilterProduct_SearchedTermDoesNotExist_ReturnsEmptyList()
        {
            ProductService service = new ProductService();

            string searchPhrase = "Terminator";

            List<Product> products = new List<Product>
            {
                new Product{ ProductCode = "P001", Name = "Milk", Description = "Milk description", SupplierId = 1, ReorderLevel = 5, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P002", Name = "Coca-Cola", Description = "Coca-Cola description", SupplierId = 2, ReorderLevel = 2, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P003", Name = "Fanta", Description = "Fanta description", SupplierId = 3, ReorderLevel = 7, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P004", Name = "Water", Description = "Water description", SupplierId = 4, ReorderLevel = 8, IsActive = true, CreatedAt = new System.DateTime()},
            };

            List<Product> filteredProducts = service.FilterProducts(products, 0, searchPhrase);

            Assert.Empty(filteredProducts);
        }

        [Fact]
        public void FilterProduct_NoFilters_ReturnsSameList()
        {
            ProductService service = new ProductService();

            List<Product> products = new List<Product>
            {
                new Product{ ProductCode = "P001", Name = "Milk", Description = "Milk description", SupplierId = 1, ReorderLevel = 5, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P002", Name = "Coca-Cola", Description = "Coca-Cola description", SupplierId = 2, ReorderLevel = 2, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P003", Name = "Fanta", Description = "Fanta description", SupplierId = 3, ReorderLevel = 7, IsActive = true, CreatedAt = new System.DateTime()},
                new Product{ ProductCode = "P004", Name = "Water", Description = "Water description", SupplierId = 4, ReorderLevel = 8, IsActive = true, CreatedAt = new System.DateTime()},
            };

            List<Product> filteredProducts = service.FilterProducts(products, 0, null);

            Assert.Equal(filteredProducts, products);
        }

        [Fact]
        public void AddProduct_ValidProduct_ReturnsTrue()
        {
            var product = new Product
            {
                ProductCode = "P001",
                Name = "Milk",
                ReorderLevel = 5
            };

            var fakeRepository = A.Fake<IProductCRUDRepository>();
            A.CallTo(() => fakeRepository.Add(product, true)).Returns(1);

            var service = new ProductService(fakeRepository);

            var result = service.AddProduct(product);

            Assert.True(result);
        }

        [Fact]
        public void ToggleProductActiveStatus_ProvidedProduct_ReturnsTrue()
        {
            Product product = new Product
            {
                ProductCode = "P001",
                Name = "Milk",
                Description = "Milk description",
                SupplierId = 1,
                ReorderLevel = 5,
                IsActive = true,
                CreatedAt = new System.DateTime()
            };

            var fakeRepository = A.Fake<IProductCRUDRepository>();
            A.CallTo(() => fakeRepository.Update(product, true)).Returns(1);

            ProductService service = new ProductService(fakeRepository);

            bool result = service.ToggleProductActiveStatus(product);

            Assert.True(result);
            Assert.False(product.IsActive);
            A.CallTo(() => fakeRepository.Update(product, true)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void ToggleProductActiveStatus_NullProduct_ReturnsFalse()
        {
            var fakeRepository = A.Fake<IProductCRUDRepository>();
            ProductService service = new ProductService(fakeRepository);

            Assert.False(service.ToggleProductActiveStatus(null));
            A.CallTo(fakeRepository).MustNotHaveHappened();
        }
    }
}
