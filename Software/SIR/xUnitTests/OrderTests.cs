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

        [Fact]
        public void GetOrders_ReturnsAllOrders()
        {
            var fakeRepo = A.Fake<IOrderCRUDRepository>();
            var service = new OrderService(fakeRepo);

            var orders = new List<Order>
            {
                new Order { Id = 1 },
                new Order { Id = 2 }
            }.AsQueryable();

            A.CallTo(() => fakeRepo.GetAll()).Returns(orders);

            var result = service.GetOrders();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetOrders_WhenNoOrdersExist_ReturnsEmptyList()
        {
            var fakeRepo = A.Fake<IOrderCRUDRepository>();
            var service = new OrderService(fakeRepo);

            A.CallTo(() => fakeRepo.GetAll()).Returns(new List<Order>().AsQueryable());

            var result = service.GetOrders();

            Assert.Empty(result);
        }


        [Fact]
        public void AddOrder_WhenOrderIsValid_ReturnsTrue()
        {
            var fakeRepo = A.Fake<IOrderCRUDRepository>();
            var service = new OrderService(fakeRepo);

            var validOrder = new Order {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.Add(validOrder, true)).Returns(1);

            var result = service.AddOrder(validOrder);

            Assert.True(result);
            A.CallTo(() => fakeRepo.Add(validOrder, true)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void AddOrder_WhenRepositoryAffectsNoRows_ReturnsFalse()
        {
            var fakeRepo = A.Fake<IOrderCRUDRepository>();
            var service = new OrderService(fakeRepo);

            var validOrder = new Order {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.Add(validOrder, true)).Returns(0);

            var result = service.AddOrder(validOrder);

            Assert.False(result);
        }

        [Fact]
        public void UpdateOrder_WhenOrderIsValid_ReturnsTrue()
        {
            var fakeRepo = A.Fake<IOrderCRUDRepository>();
            var service = new OrderService(fakeRepo);

            var validOrder = new Order {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.Update(validOrder, true)).Returns(1);

            var result = service.UpdateOrder(validOrder);

            Assert.True(result);
            A.CallTo(() => fakeRepo.Update(validOrder, true)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void UpdateOrder_WhenRepositoryAffectsNoRows_ReturnsFalse()
        {
            var fakeRepo = A.Fake<IOrderCRUDRepository>();
            var service = new OrderService(fakeRepo);

            var validOrder = new Order {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now, 
                ReceivedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.Update(validOrder, true)).Returns(0);

            var result = service.UpdateOrder(validOrder);

            Assert.False(result);
        }


        [Fact]
        public void RemoveOrder_WhenOrderIsNull_ReturnsFalse()
        {
            var fakeRepo = A.Fake<IOrderCRUDRepository>();
            var service = new OrderService(fakeRepo);

            var result = service.RemoveOrder(null);

            Assert.False(result);
            A.CallTo(() => fakeRepo.Remove(A<Order>._, A<bool>._)).MustNotHaveHappened();
        }

        [Fact]
        public void RemoveOrder_WhenOrderIsRemoved_ReturnsTrue()
        {
            var fakeRepo = A.Fake<IOrderCRUDRepository>();
            var service = new OrderService(fakeRepo);

            var order = new Order { Id = 1 };
            A.CallTo(() => fakeRepo.Remove(order, true)).Returns(1);

            var result = service.RemoveOrder(order);

            Assert.True(result);
            A.CallTo(() => fakeRepo.Remove(order, true)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void RemoveOrder_WhenRepositoryAffectsNoRows_ReturnsFalse()
        {
            var fakeRepo = A.Fake<IOrderCRUDRepository>();
            var service = new OrderService(fakeRepo);

            var order = new Order { Id = 1 };
            A.CallTo(() => fakeRepo.Remove(order, true)).Returns(0);

            var result = service.RemoveOrder(order);

            Assert.False(result);
        }
    }
}
