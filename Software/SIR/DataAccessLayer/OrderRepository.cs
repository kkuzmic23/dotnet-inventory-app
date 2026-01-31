using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class OrderRepository
    {
        public List<Order> GetAll()
        {
            using (var context = new Model1())
            {
                return context.Orders.ToList();
            }
        }

        public int Add(Order order)
        {
            using (var context = new Model1())
            {
                context.Orders.Add(order);
                return context.SaveChanges();
            }
        }

        public int Update(Order order)
        {
            using (var context = new Model1())
            {
                var existing = context.Orders.FirstOrDefault(o => o.Id == order.Id);
                if (existing == null)
                {
                    return 0;
                }

                existing.SupplierId = order.SupplierId;
                existing.Status = order.Status;
                existing.CreatedAt = order.CreatedAt;
                existing.ReceivedAt = order.ReceivedAt;

                return context.SaveChanges();
            }
        }

        public int Remove(Order order)
        {
            using (var context = new Model1())
            {
                var existing = context.Orders.FirstOrDefault(o => o.Id == order.Id);
                if (existing == null)
                {
                    return 0;
                }

                context.Orders.Remove(existing);
                return context.SaveChanges();
            }
        }
    }
}
