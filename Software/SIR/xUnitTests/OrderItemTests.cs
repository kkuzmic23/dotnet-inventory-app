using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using FakeItEasy;
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

        [Fact]
        public void GetItemsByOrderId_ReturnsOnlyItemsMatchingOrderId()
        {
            var fakeRepo = A.Fake<IOrderItemCRUDRepository>();
            var service = new OrderItemService(fakeRepo);

            var allItems = new List<OrderHasProduct>
        {
            new OrderHasProduct { OrderId = 1, ProductId = 10 },
            new OrderHasProduct { OrderId = 1, ProductId = 11 },
            new OrderHasProduct { OrderId = 2, ProductId = 20 }
        };
            A.CallTo(() => fakeRepo.GetAll()).Returns(allItems.AsQueryable());

            var result = service.GetItemsByOrderId(1);

            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Equal(1, item.OrderId));
        }

        [Fact]
        public void GetItemsByOrderId_WhenNoItemsMatchOrderId_ReturnsEmptyList()
        {
            var fakeRepo = A.Fake<IOrderItemCRUDRepository>();
            var service = new OrderItemService(fakeRepo);

            A.CallTo(() => fakeRepo.GetAll()).Returns(new List<OrderHasProduct>().AsQueryable());

            var result = service.GetItemsByOrderId(99);

            Assert.Empty(result);
        }

        [Fact]
        public void ReplaceItems_WhenValidationFails_ReturnsFalse()
        {
            var fakeRepo = A.Fake<IOrderItemCRUDRepository>();
            var service = new OrderItemService(fakeRepo);

            var invalidItems = new List<OrderHasProduct>
            {
                new OrderHasProduct {
                    OrderId = 1,
                    ProductId = 0,
                    Quantity = -1
                }
            };

            var result = service.ReplaceItems(1, invalidItems);

            Assert.False(result);
            A.CallTo(() => fakeRepo.RemoveByOrderId(A<int>._)).MustNotHaveHappened();
            A.CallTo(() => fakeRepo.Add(A<OrderHasProduct>._, true)).MustNotHaveHappened();
            A.CallTo(() => fakeRepo.SaveChanges()).MustNotHaveHappened();
        }

        [Fact]
        public void ReplaceItems_WhenValidationPasses_RemovesOldItems_AndAddNewOnes()
        {
            var fakeRepo = A.Fake<IOrderItemCRUDRepository>();
            var service = new OrderItemService(fakeRepo);

            var items = new List<OrderHasProduct>
        {
            new OrderHasProduct {OrderId = 1, ProductId = 10, Quantity = 2 },
            new OrderHasProduct { OrderId = 1, ProductId = 11, Quantity = 1 }
        };

            var result = service.ReplaceItems(1, items);

            Assert.True(result);
            A.CallTo(() => fakeRepo.RemoveByOrderId(1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeRepo.Add(A<OrderHasProduct>._, true)).MustHaveHappenedANumberOfTimesMatching(n => n == 2);
            A.CallTo(() => fakeRepo.SaveChanges()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void ReplaceItems_WhenItemsAreSuccessfullyReplaced_ReturnsTrue()
        {
            var fakeRepo = A.Fake<IOrderItemCRUDRepository>();
            var service = new OrderItemService(fakeRepo);

            var items = new List<OrderHasProduct>
            {
                new OrderHasProduct {
                    OrderId = 1,
                    ProductId = 10,
                    Quantity = 2
                }
            };

            var result = service.ReplaceItems(1, items);

            Assert.True(result);
        }
    }
}
