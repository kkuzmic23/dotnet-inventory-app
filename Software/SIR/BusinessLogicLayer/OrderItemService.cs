using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class OrderItemService
    {
        public List<OrderHasProduct> GetItemsByOrderId(int orderId)
        {
            using (var repo = new OrderHasProductRepository())
            {
                return repo.GetAll().Where(x => x.OrderId == orderId).ToList();
            }
        }

        public bool ReplaceItems(int orderId, IEnumerable<OrderHasProduct> items)
        {
            using (var repo = new OrderHasProductRepository())
            {
                repo.RemoveByOrderId(orderId);

                foreach(var item in items)
                {
                    repo.Add(item);
                }

                return repo.SaveChanges() > 0;
            }
        }
    }
}
