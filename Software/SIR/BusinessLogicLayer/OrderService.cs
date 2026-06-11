using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class OrderService
    {
        public List<Order> GetOrders()
        {
            using (var repo = new OrderRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public bool AddOrder(Order order)
        {
            if (!ValidateOrder(order).IsSuccessful)
            {
                return false;
            }

            using (var repo = new OrderRepository())
            {
                int affectedRows = repo.Add(order);
                return affectedRows > 0;
            }
        }

        public bool UpdateOrder(Order order)
        {
            if (!ValidateOrder(order).IsSuccessful)
            {
                return false;
            }

            using (var repo = new OrderRepository())
            {
                int affectedRows = repo.Update(order);
                return affectedRows > 0;
            }
        }

        public bool RemoveOrder(Order order)
        {
            if (order == null)
            {
                return false;
            }

            using (var repo = new OrderRepository())
            {
                int affectedRows = repo.Remove(order);
                return affectedRows > 0;
            }
        }

        public ServiceResult ValidateOrder(Order order)
        {
            if (order == null)
            {
                return ServiceResult.Failure("Order is required");
            }

            if (order.SupplierId <= 0)
            {
                return ServiceResult.Failure("Select a supplier first");
            }

            if (string.IsNullOrWhiteSpace(order.Status))
            {
                return ServiceResult.Failure("Status is required");
            }

            if (order.CreatedAt == default(System.DateTime))
            {
                return ServiceResult.Failure("Created date is required");
            }

            return ServiceResult.Success();
        }
    }
}
