using EntityLayer.Entities;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public class StockExportRepository : Repository<StockExport>
    {
        public StockExportRepository() : base(new Model1())
        {
        }

        public override IQueryable<StockExport> GetAll()
        {
            return Entities.Include(e => e.StockExportHasProducts);
        }

        public override int Update(StockExport export, bool saveChanges = true)
        {
            var existing = Entities.SingleOrDefault(e => e.Id == export.Id);
            if (existing == null)
            {
                return 0;
            }

            existing.CreatedAt = export.CreatedAt;
            existing.Notes = export.Notes;
            return saveChanges ? SaveChanges() : 0;
        }
    }
}
