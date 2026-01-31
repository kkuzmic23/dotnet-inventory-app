using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository() : base(new Model1())
        {

        }

        public override IQueryable<Order> GetAll()
        {
            var query = from o in Entities.Include("Supplier")
                        select o;
            return query;
        }

        public override int Update(Order entity, bool saveChanges = true)
        {
            var order = Entities.SingleOrDefault(o => o.Id == entity.Id);

            order.SupplierId = entity.SupplierId;
            order.Status = entity.Status;
            order.CreatedAt = entity.CreatedAt;
            order.ReceivedAt = entity.ReceivedAt;

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }
    }
}
