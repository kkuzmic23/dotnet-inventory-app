using EntityLayer.Entities;
using System.Linq;

namespace DataAccessLayer
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository() : base(new Model1())
        {
        }

        public override int Update(Order order, bool saveChanges = true)
        {
            var existing = Entities.SingleOrDefault(o => o.Id == order.Id);
            if (existing == null)
            {
                return 0;
            }

            existing.SupplierId = order.SupplierId;
            existing.Status = order.Status;
            existing.CreatedAt = order.CreatedAt;
            existing.ReceivedAt = order.ReceivedAt;

            return saveChanges ? SaveChanges() : 0;
        }
    }
}