using BusinessLogicLayer;
using EntityLayer.Entities;
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
    }
}
