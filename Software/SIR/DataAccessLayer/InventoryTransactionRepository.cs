using EntityLayer.Entities;
using System.Linq;

namespace DataAccessLayer
{
    public class InventoryTransactionRepository : Repository<InventoryTransaction>
    {
        public InventoryTransactionRepository() : base(new Model1())
        {
        }

        public override int Update(InventoryTransaction transaction, bool saveChanges = true)
        {
            var existing = Entities.SingleOrDefault(t => t.Id == transaction.Id);
            if (existing == null)
            {
                return 0;
            }

            existing.ProductId = transaction.ProductId;
            existing.QuantityDelta = transaction.QuantityDelta;
            existing.Type = transaction.Type;
            existing.ReferenceId = transaction.ReferenceId;
            existing.Notes = transaction.Notes;
            existing.CreatedAt = transaction.CreatedAt;

            return saveChanges ? SaveChanges() : 0;
        }
    }
}
