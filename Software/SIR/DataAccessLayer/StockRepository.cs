using EntityLayer.Entities;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public class StockRepository : Repository<Stock>
    {
        public StockRepository() : base(new Model1())
        {
        }

        public override IQueryable<Stock> GetAll()
        {
            return Entities
                .Include(s => s.Product)
                .Include(s => s.Product.Supplier);
        }

        public Stock GetByProductId(int productId)
        {
            return Entities.SingleOrDefault(s => s.ProductId == productId);
        }

        public override int Update(Stock stock, bool saveChanges = true)
        {
            var existing = Entities.SingleOrDefault(s => s.ProductId == stock.ProductId);
            if (existing == null)
            {
                return 0;
            }

            existing.Quantity = stock.Quantity;
            return saveChanges ? SaveChanges() : 0;
        }
    }
}
