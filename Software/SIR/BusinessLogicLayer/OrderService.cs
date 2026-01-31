using DataAccessLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            bool isSuccessful = false;
            if (order == null)
            {
                return false;
            }

            using (var repo = new OrderRepository())
            {
                int affectedRows = repo.Add(order);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public bool UpdateOrder(Order order)
        {
            bool isSuccessful = false;
            if (order == null)
            {
                return false;
            }

            using (var repo = new OrderRepository())
            {
                int affectedRows = repo.Update(order);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public bool RemoveOrder(Order order)
        {
            bool isSuccessful = false;
            if (order == null)
            {
                return false;
            }

            using (var repo = new OrderRepository())
            {
                int affectedRows = repo.Remove(order);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }
    }
}
