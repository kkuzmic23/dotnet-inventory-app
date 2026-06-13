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
        private readonly List<int> insertedOrders = new List<int>();

        [Fact]
        public void AddOrder_GivenValidOrder_PersistsToDB()
        {
            OrderService service = new OrderService();

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
            insertedOrders.Add(order.Id);
        }

        [Fact]
        public void AddOrder_GivenInvalidOrder_DoesNotPersistToDB()
        {
            OrderService service = new OrderService();

            var invalidOrder = new Order();

            var result = service.AddOrder(invalidOrder);

            Assert.False(result);
            Assert.Equal(0, invalidOrder.Id);
        }

        [Fact]
        public void GetOrders_ReturnsAtLeastOneOrder()
        {
            OrderService service = new OrderService();

            var order = new Order
            {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };
            service.AddOrder(order);
            insertedOrders.Add(order.Id);

            var result = service.GetOrders();

            Assert.NotEmpty(result);
        }

        [Fact]
        public void UpdateOrder_WhenOrderExists_UpdatesInDB()
        {
            OrderService service = new OrderService();

            var order = new Order
            {
                SupplierId = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                ReceivedAt = DateTime.Now
            };
            service.AddOrder(order);
            insertedOrders.Add(order.Id);

            order.Status = "Inactive";
            var result = service.UpdateOrder(order);

            Assert.True(result);

            var updated = service.GetOrders().FirstOrDefault(o => o.Id == order.Id);
            Assert.Equal("Inactive", updated.Status);
        }

        [Fact]
        public void UpdateOrder_GivenInvalidOrder_ReturnsFailure()
        {
            OrderService service = new OrderService();

            var invalidOrder = new Order();

            var result = service.UpdateOrder(invalidOrder);

            Assert.False(result);
            Assert.Equal(0, invalidOrder.Id);
        }

        [Fact]
        public void RemoveOrder_WhenOrderExists_DeletesFromDB()
        {
            OrderService service = new OrderService();

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
            OrderService service = new OrderService();

            var result = service.RemoveOrder(null);

            Assert.False(result);
        }

        public void Dispose()
        {
            OrderService service = new OrderService();

            foreach (var id in insertedOrders)
            {
                var order = service.GetOrders().FirstOrDefault(o => o.Id == id);
                if (order != null)
                {
                    service.RemoveOrder(order);
                }
            }
        }
    }
}
