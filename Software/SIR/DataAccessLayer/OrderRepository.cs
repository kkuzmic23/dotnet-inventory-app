using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public int SetStatusForOrders(IEnumerable<int> orderIds, string status, bool saveChanges = true)
        {
            var orders = Entities.Where(o => orderIds.Contains(o.Id)).ToList();
            foreach (var order in orders)
            {
                order.Status = status;
            }

            return saveChanges ? SaveChanges() : 0;
        }
        public int CountCreated(DateTime from, DateTime toExclusive) {
            return Entities.Count(o => o.CreatedAt >= from && o.CreatedAt < toExclusive);
        }

        public int SumReceivedUnits(DateTime from, DateTime toExclusive) {
            int? sum = Entities
                .Where(o => o.ReceivedAt != null &&
                            o.ReceivedAt >= from &&
                            o.ReceivedAt < toExclusive)
                .SelectMany(o => o.OrderHasProducts.Select(x => (int?)x.Quantity))
                .Sum();

            return sum ?? 0;
        }

        public double AvgLeadTimeDays(DateTime from, DateTime toExclusive) {
            double? avg = Entities
                .Where(o => o.ReceivedAt != null &&
                            o.ReceivedAt >= from &&
                            o.ReceivedAt < toExclusive)
                .Select(o => (double?)DbFunctions.DiffDays(o.CreatedAt, o.ReceivedAt.Value))
                .Average();

            return avg ?? 0.0;
        }

        public List<DailyPoint> ReceivedUnitsByDay(DateTime from, DateTime toExclusive) {
            var rows = Entities
                .Where(o => o.ReceivedAt != null &&
                            o.ReceivedAt >= from &&
                            o.ReceivedAt < toExclusive)
                .SelectMany(o => o.OrderHasProducts.Select(p => new {
                    Day = DbFunctions.TruncateTime(o.ReceivedAt.Value),
                    Qty = p.Quantity
                }))
                .GroupBy(x => x.Day)
                .Select(g => new { Day = g.Key.Value, Qty = g.Sum(z => z.Qty) })
                .OrderBy(x => x.Day)
                .ToList();

            return rows.Select(x => new DailyPoint { Day = x.Day, Value = x.Qty }).ToList();
        }

        public List<BreakdownRow> TopSuppliersByCreatedOrders(DateTime from, DateTime toExclusive, int take) {
            var rows = Entities
                .Where(o => o.CreatedAt >= from && o.CreatedAt < toExclusive)
                .GroupBy(o => o.Supplier.Name)
                .Select(g => new { Name = g.Key, Cnt = g.Count() })
                .OrderByDescending(x => x.Cnt)
                .Take(take)
                .ToList();

            int total = rows.Sum(x => x.Cnt);

            return rows.Select(x => new BreakdownRow {
                Name = x.Name,
                Value = x.Cnt,
                Percent = total == 0 ? 0 : x.Cnt * 100.0 / total
            }).ToList();
        }

        public List<DailyProductQty> IncomingByDayByProduct(DateTime from, DateTime toExclusive) {
            var rows = Entities
                .Where(o => o.ReceivedAt != null &&
                            o.ReceivedAt >= from && o.ReceivedAt < toExclusive)
                .SelectMany(o => o.OrderHasProducts.Select(op => new {
                    Day = DbFunctions.TruncateTime(o.ReceivedAt.Value),
                    ProductName = op.Product.Name,
                    Qty = op.Quantity
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
