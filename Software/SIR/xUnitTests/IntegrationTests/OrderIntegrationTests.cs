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
        public void AddOrder_WhenValidOrder_PersistsToDB()
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
