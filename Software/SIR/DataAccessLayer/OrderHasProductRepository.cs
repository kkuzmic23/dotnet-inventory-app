using EntityLayer.Entities;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public class OrderHasProductRepository : Repository<OrderHasProduct>
    {
        public OrderHasProductRepository() : base(new Model1())
        {
        }

        public override IQueryable<OrderHasProduct> GetAll()
        {
            return Entities
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Include(x => x.Order.Supplier);
        }

        public override int Update(OrderHasProduct item, bool saveChanges = true)
        {
            var existing = Entities.SingleOrDefault(o => o.Id == item.Id);
            if (existing == null)
            {
                return 0;
            }

            existing.OrderId = item.OrderId;
            existing.ProductId = item.ProductId;
            existing.Quantity = item.Quantity;

            return saveChanges ? SaveChanges() : 0;
        }
    }
}
