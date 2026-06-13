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
    public class OrderIntegrationTests : IDisposable
    {
        private readonly OrderRepository repo = new OrderRepository();
        private readonly OrderService service = new OrderService();
        private readonly List<Order> insertedOrders = new List<Order>();

        [Fact]
        public void AddOrder_GivenValidOrder_PersistsToDB()
        {
            var order = new Order
            {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };

            var result = service.AddOrder(order);

            Assert.True(result);
            Assert.True(order.Id > 0);
            insertedOrders.Add(order);
        }

        [Fact]
        public void AddOrder_GivenInvalidOrder_DoesNotPersistToDB()
        {
            var invalidOrder = new Order();

            var result = service.AddOrder(invalidOrder);

            Assert.False(result);
            Assert.Equal(0, invalidOrder.Id);
        }

        [Fact]
        public void GetOrders_ReturnsAtLeastOneOrder()
        {
            var order = new Order
            {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };
            service.AddOrder(order);
            insertedOrders.Add(order);

            var result = service.GetOrders();

            Assert.NotEmpty(result);
        }

        [Fact]
        public void UpdateOrder_WhenOrderExists_UpdatesInDB()
        {
            var order = new Order
            {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };
            service.AddOrder(order);
            insertedOrders.Add(order);

            order.Status = "Inactive";
            var result = service.UpdateOrder(order);

            Assert.True(result);

            var updated = service.GetOrders().FirstOrDefault(o => o.Id == order.Id);
            Assert.Equal("Inactive", updated.Status);
        }

        [Fact]
        public void UpdateOrder_GivenInvalidOrder_ReturnsFailure()
        {
            var invalidOrder = new Order();

            var result = service.UpdateOrder(invalidOrder);

            Assert.False(result);
            Assert.Equal(0, invalidOrder.Id);
        }

        [Fact]
        public void RemoveOrder_WhenOrderExists_DeletesFromDB()
        {
            var order = new Order
            {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };
            service.AddOrder(order);

            var result = service.RemoveOrder(order);

            Assert.True(result);

            var found = service.GetOrders().FirstOrDefault(o => o.Id == order.Id);
            Assert.Null(found);
        }

        [Fact]
        public void RemoveOrder_GivenInvalidOrder_ReturnsFailure()
        {
            var invalidOrder = new Order();

            var result = service.RemoveOrder(invalidOrder);

            Assert.False(result);
            Assert.Equal(0, invalidOrder.Id);
        }

        public void Dispose()
        {
            foreach (var order in insertedOrders)
            {
                repo.Remove(order);
            }
            repo.Dispose();
        }
    }
}
