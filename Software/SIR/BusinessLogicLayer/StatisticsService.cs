using DataAccessLayer;
using EntityLayer.Entities;
using System;
using System.Threading.Tasks;

namespace BusinessLogicLayer {
    public class StatisticsService {
        public Task<StatisticsSummary> GetSummaryAsync(DateTime from, DateTime to) {
            DateTime fromDate = from.Date;
            DateTime toExclusive = to.Date.AddDays(1);

            return Task.Run(() => {
                using (var orderRepo = new OrderRepository())
                using (var exportRepo = new StockExportRepository())
                using (var stockRepo = new StockRepository()) {
                    var s = new StatisticsSummary();

                    s.TotalOrders = orderRepo.CountCreated(fromDate, toExclusive);
                    s.IncomingUnits = orderRepo.SumReceivedUnits(fromDate, toExclusive);
                    s.AvgLeadTimeDays = orderRepo.AvgLeadTimeDays(fromDate, toExclusive);
                    s.LowStockCount = stockRepo.CountLowStock();

                    s.IncomingByDayByProduct = orderRepo.IncomingByDayByProduct(fromDate, toExclusive);
                    s.OutgoingByDayByProduct = exportRepo.OutgoingByDayByProduct(fromDate, toExclusive);

                    s.TopSuppliers = orderRepo.TopSuppliersByCreatedOrders(fromDate, toExclusive, 5);
                    s.TopProductsExport = exportRepo.TopProductsByOutgoingQty(fromDate, toExclusive, 5);

                    return s;
                }
            });
        }
    }
}
