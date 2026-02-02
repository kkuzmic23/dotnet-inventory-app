using EntityLayer.Entities;
using System;
using System.Collections.Generic;
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

        public List<ActivityRow> Latest(DateTime from, DateTime toExclusive, int take) {
            return Entities
                .Include("Product")
                .Where(t => t.CreatedAt >= from && t.CreatedAt < toExclusive)
                .OrderByDescending(t => t.CreatedAt)
                .Take(take)
                .Select(t => new ActivityRow {
                    CreatedAt = t.CreatedAt,
                    Type = t.Type,
                    ProductName = t.Product.Name,
                    QuantityDelta = t.QuantityDelta,
                    ReferenceId = t.ReferenceId.ToString(),
                    Notes = t.Notes
                })
                .ToList();
        }
    }
}
