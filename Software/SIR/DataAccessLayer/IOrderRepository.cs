using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public interface IOrderRepository : IOrderCRUDRepository
    {
        int SetStatusForOrders(IEnumerable<int> orderIds, string status, bool saveChanges = true);
        int SaveChanges();
    }
}
