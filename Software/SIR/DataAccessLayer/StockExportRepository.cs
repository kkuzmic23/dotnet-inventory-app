using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public class StockExportRepository : Repository<StockExport>, IStockExportRepository
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

        public List<BreakdownRow> TopProductsByOutgoingQty(DateTime from, DateTime toExclusive, int take) {
            var rows = Entities
                .Where(se => se.CreatedAt >= from && se.CreatedAt < toExclusive)
                .SelectMany(se => se.StockExportHasProducts.Select(p => new {
                    ProductName = p.Product.Name,
                    Qty = p.Quantity
                }))
                .GroupBy(x => x.ProductName)
                .Select(g => new { Name = g.Key, Qty = g.Sum(z => z.Qty) })
                .OrderByDescending(x => x.Qty)
                .Take(take)
                .ToList();

            int total = rows.Sum(x => x.Qty);

            return rows.Select(x => new BreakdownRow {
                Name = x.Name,
                Value = x.Qty,
                Percent = total == 0 ? 0 : x.Qty * 100.0 / total
            }).ToList();
        }
        public List<DailyProductQty> OutgoingByDayByProduct(DateTime from, DateTime toExclusive) {
            var rows = Entities
                .Where(se => se.CreatedAt >= from && se.CreatedAt < toExclusive)
                .SelectMany(se => se.StockExportHasProducts.Select(sp => new {
                    Day = DbFunctions.TruncateTime(se.CreatedAt),
                    ProductName = sp.Product.Name,
                    Qty = sp.Quantity
                }))
                .GroupBy(x => new { x.Day, x.ProductName })
                .Select(g => new {
                    Day = g.Key.Day.Value,
                    Name = g.Key.ProductName,
                    Qty = g.Sum(z => z.Qty)
                })
                .OrderBy(x => x.Day)
                .ToList();

            return rows.Select(x => new DailyProductQty {
                Day = x.Day,
                ProductName = x.Name,
                Qty = x.Qty
            }).ToList();
        }
    }
}
