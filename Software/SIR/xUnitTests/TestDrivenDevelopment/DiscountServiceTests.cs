using BusinessLogicLayer;
using Xunit;

namespace xUnitTests.TestDrivenDevelopment
{
    public class DiscountServiceTests
    {
        [Fact]
        public void CalculatePrice_WithTenPercentDiscount_ReturnsCorrectAmount()
        {
            // Arrange
            var service = new DiscountService();
            double originalPrice = 100.0;
            double expectedPrice = 90.0;

            // Act
            double actualPrice = service.CalculatePrice(originalPrice);

            // Assert
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void CalculatePrice_WithAmountOverFiveHundred_ReturnsTwentyPercentDiscount()
        {
            // Arrange
            var service = new DiscountService();
            double originalPrice = 600.0;
            double expectedPrice = 480.0; 

            // Act
            double actualPrice = service.CalculatePrice(originalPrice);

            // Assert
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void CalculatePrice_WithNegativeAmount_ThrowsArgumentException()
        {
            // Arrange
            var service = new DiscountService();

            // Act & Assert
            Assert.Throws<System.ArgumentException>(() => service.CalculatePrice(-10.0));
        }
    }
}
