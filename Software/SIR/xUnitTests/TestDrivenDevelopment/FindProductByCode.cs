using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTests.TestDrivenDevelopment
{
    public class FindProductByCodeTests
    {
        [Fact]
        public void FindProductByCode_ExactCode_ReturnsMatchingProduct()
        {
            Product product = new Product
            {
                ProductCode = "P001",
                Name = "Milk"
            };

            var fakeRepository = A.Fake<IProductCRUDRepository>();
            A.CallTo(() => fakeRepository.GetAll()).Returns(new List<Product> { product }.AsQueryable());

            ProductService service = new ProductService(fakeRepository);

            Product result = service.FindProductByCode("P001");

            Assert.Equal(product, result);
        }

        [Fact]
        public void FindProductByCode_IgnoresCaseAndWhitespace_ReturnsMatchingProduct()
        {
            Product product = new Product
            {
                ProductCode = "P001",
                Name = "Milk"
            };

            var fakeRepository = A.Fake<IProductCRUDRepository>();
            A.CallTo(() => fakeRepository.GetAll()).Returns(new List<Product> { product }.AsQueryable());

            ProductService service = new ProductService(fakeRepository);

            Product result = service.FindProductByCode("  p001  ");

            Assert.Equal(product, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void FindProductByCode_BlankCode_ReturnsNull(string productCode)
        {
            var fakeRepository = A.Fake<IProductCRUDRepository>();
            ProductService service = new ProductService(fakeRepository);

            Product result = service.FindProductByCode(productCode);

            Assert.Null(result);
            A.CallTo(() => fakeRepository.GetAll()).MustNotHaveHappened();
        }

        [Fact]
        public void FindProductByCode_NoMatch_ReturnsNull()
        {
            var fakeRepository = A.Fake<IProductCRUDRepository>();
            A.CallTo(() => fakeRepository.GetAll()).Returns(new List<Product>
            {
                new Product { ProductCode = "P001", Name = "Milk" },
                new Product { ProductCode = "P002", Name = "Bread" }
            }.AsQueryable());

            ProductService service = new ProductService(fakeRepository);

            Product result = service.FindProductByCode("P999");

            Assert.Null(result);
        }
    }
}
