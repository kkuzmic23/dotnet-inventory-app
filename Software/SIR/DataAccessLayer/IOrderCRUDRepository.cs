using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IOrderCRUDRepository : IDisposable
    {
        IQueryable<Order> GetAll();

        int Add(Order order, bool saveChanges = true);

        int Update(Order order, bool saveChanges = true);

        int Remove(Order order, bool saveChanges = true);
    }
}
