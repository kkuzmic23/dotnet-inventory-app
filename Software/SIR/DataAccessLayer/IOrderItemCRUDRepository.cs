using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IOrderItemCRUDRepository : IDisposable
    {
        IQueryable<OrderHasProduct> GetAll();

        int Add(OrderHasProduct item, bool saveChanges = true);

        int RemoveByOrderId(int orderId);

        int SaveChanges();
    }
}
