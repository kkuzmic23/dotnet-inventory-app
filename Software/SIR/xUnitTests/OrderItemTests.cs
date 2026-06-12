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
    public class OrderItemTests
    {
        [Fact]
        public void ValidateOrderItems_ValidOrderItems_ReturnsSuccess()
        {
            OrderItemService service = new OrderItemService();

            List<OrderHasProduct> orders = new List<OrderHasProduct>
            {
                new OrderHasProduct { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1},
                new OrderHasProduct { Id = 2, OrderId = 2, ProductId = 2, Quantity = 2},
                new OrderHasProduct { Id = 3, OrderId = 3, ProductId = 3, Quantity = 3},
                new OrderHasProduct { Id = 4, OrderId = 4, ProductId = 4, Quantity = 4},
                new OrderHasProduct { Id = 5, OrderId = 5, ProductId = 5, Quantity = 5},
                new OrderHasProduct { Id = 6, OrderId = 6, ProductId = 6, Quantity = 6},
            };

            ServiceResult result = service.ValidateOrderItems(orders);

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ValidateOrderItems_InvalidProductId_ReturnsFailure()
        {
            OrderItemService service = new OrderItemService();

            List<OrderHasProduct> orders = new List<OrderHasProduct>
            {
                new OrderHasProduct { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1},
                new OrderHasProduct { Id = 2, OrderId = 2, ProductId = 2, Quantity = 2},
                new OrderHasProduct { Id = 3, OrderId = 3, ProductId = 3, Quantity = 3},
                new OrderHasProduct { Id = 4, OrderId = 4, ProductId = 4, Quantity = 4},
                new OrderHasProduct { Id = 5, OrderId = 5, ProductId = -6, Quantity = 5}, // invalid ProductId
                new OrderHasProduct { Id = 6, OrderId = 6, ProductId = 6, Quantity = 6},
            };

            ServiceResult result = service.ValidateOrderItems(orders);

            Assert.False(result.IsSuccessful);
            Assert.Equal("An order item cannot be empty", result.ErrorMessage);
        }

        [Fact]
        public void ValidateOrderItems_DuplicateProducts_ReturnsFailure()
        {
            OrderItemService service = new OrderItemService();

            List<OrderHasProduct> orders = new List<OrderHasProduct>
            {
                new OrderHasProduct { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1},
                new OrderHasProduct { Id = 2, OrderId = 2, ProductId = 2, Quantity = 2},
                new OrderHasProduct { Id = 3, OrderId = 3, ProductId = 3, Quantity = 3},
                new OrderHasProduct { Id = 4, OrderId = 4, ProductId = 4, Quantity = 4},
                new OrderHasProduct { Id = 5, OrderId = 5, ProductId = 3, Quantity = 5}, // duplicate ProductId
                new OrderHasProduct { Id = 6, OrderId = 6, ProductId = 6, Quantity = 6},
            };

            ServiceResult result = service.ValidateOrderItems(orders);

            Assert.False(result.IsSuccessful);
            Assert.Equal("Cannot have the same product more than once", result.ErrorMessage);
        }

        [Fact]
        public void ValidateOrderItems_EmptyQuantity_ReturnsFailure()
        {
            OrderItemService service = new OrderItemService();

            List<OrderHasProduct> orders = new List<OrderHasProduct>
            {
                new OrderHasProduct { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1},
                new OrderHasProduct { Id = 2, OrderId = 2, ProductId = 2, Quantity = 2},
                new OrderHasProduct { Id = 3, OrderId = 3, ProductId = 3, Quantity = 3},
                new OrderHasProduct { Id = 4, OrderId = 4, ProductId = 4, Quantity = 0},
                new OrderHasProduct { Id = 5, OrderId = 5, ProductId = 5, Quantity = -1}, // empty quantity
                new OrderHasProduct { Id = 6, OrderId = 6, ProductId = 6, Quantity = 6},
            };

            ServiceResult result = service.ValidateOrderItems(orders);

            Assert.False(result.IsSuccessful);
            Assert.Equal("An order item cannot have 0 or less", result.ErrorMessage);
        }

        [Fact]
        public void ValidateOrderItems_EmptyList_ReturnsFailure()
        {
            OrderItemService service = new OrderItemService();

            ServiceResult result = service.ValidateOrderItems(null);

            Assert.False(result.IsSuccessful);
            Assert.Equal("Add at least one item", result.ErrorMessage);
        }
    }
}
