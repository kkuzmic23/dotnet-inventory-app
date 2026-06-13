using EntityLayer.Entities;
using System.Linq;

namespace DataAccessLayer
{
    public class StockExportHasProductRepository : Repository<StockExportHasProduct>, IStockExportHasProductRepository
    {
        public StockExportHasProductRepository() : base(new Model1())
        {
        }

        public override int Update(StockExportHasProduct item, bool saveChanges = true)
        {
            var existing = Entities.SingleOrDefault(e => e.Id == item.Id);
            if (existing == null)
            {
                return 0;
            }

            existing.StockExportId = item.StockExportId;
            existing.ProductId = item.ProductId;
            existing.Quantity = item.Quantity;
            return saveChanges ? SaveChanges() : 0;
        }
    }
}
