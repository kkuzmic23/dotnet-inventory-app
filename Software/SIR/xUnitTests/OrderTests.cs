using BusinessLogicLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTests
{
    public class OrderTests
    {
        [Fact]
        public void ValidateOrder_ValidOrder_ReturnsSuccess() {
            OrderService service = new OrderService();

            Order order = new Order
            {
                Id = 67,
                SupplierId = 5,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };

            ServiceResult result = service.ValidateOrder(order);

            Assert.True(result.IsSuccessful);
        }

        [Theory]
        [InlineData(5, null)]
        [InlineData(5, "")]
        [InlineData(5, "  ")]
        [InlineData(0, "Active")]
        [InlineData(-2, "Active")]
        public void ValidateOrder_MissingRequiredFields_ReturnsFailure(int supplierId, string status)
        {
            OrderService service = new OrderService();

            Order order = new Order{
                Id = 67, 
                SupplierId = supplierId, 
                Status = status, 
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };

            ServiceResult result = service.ValidateOrder(order);

            Assert.False(result.IsSuccessful);
        }
    }
}
