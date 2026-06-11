using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class ImportService : IImportService
    {
        public List<OrderHasProduct> GetImports()
        {
            using (var repo = new OrderHasProductRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public List<SupplierImportSummary> GetImportSummaries()
        {
            using (var repo = new OrderHasProductRepository())
            {
                var items = repo.GetAll().ToList();

                return items
                    .Where(x => x.Order?.Supplier != null && x.Product != null && x.Order.Status != "Imported")
                    .GroupBy(x => new { x.Order.Supplier.Id, x.Order.Supplier.Name })
                    .Select(g => new SupplierImportSummary
                    {
                        SupplierId = g.Key.Id,
                        SupplierName = g.Key.Name,
                        Products = g
                            .GroupBy(p => new { p.Product.Id, p.Product.Name })
                            .Select(pg => new ProductImportSummary
                            {
                                ProductId = pg.Key.Id,
                                ProductName = pg.Key.Name,
                                TotalQuantity = pg.Sum(x => x.Quantity)
                            })
                            .OrderBy(p => p.ProductName)
                            .ToList()
                    })
                    .OrderBy(s => s.SupplierName)
                    .ToList();
            }
        }

        public List<ImportItemDetail> GetImportDetailsBySupplier(int supplierId)
        {
            using (var repo = new OrderHasProductRepository())
            {
                return repo.GetAll()
                    .Where(x => x.Order != null && x.Order.SupplierId == supplierId && x.Product != null && x.Order.Status != "Imported")
                    .Select(x => new ImportItemDetail
                    {
                        OrderId = x.OrderId,
                        ProductId = x.ProductId,
                        ProductCode = x.Product.ProductCode,
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity,
                        OrderStatus = x.Order.Status,
                        OrderCreatedAt = x.Order.CreatedAt,
                        OrderReceivedAt = x.Order.ReceivedAt
                    })
                    .OrderBy(x => x.OrderId)
                    .ThenBy(x => x.ProductName)
                    .ToList();
            }
        }

        public bool ApplyImport(int supplierId)
        {
            var details = GetImportDetailsBySupplier(supplierId);
            if (details.Count == 0)
            {
                return false;
            }

            var totals = details
                .GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Total = g.Sum(x => x.Quantity) })
                .ToList();

            using (var stockRepo = new StockRepository())
            using (var transactionRepo = new InventoryTransactionRepository())
            using (var orderRepo = new OrderRepository())
            {
                foreach (var total in totals)
                {
                    var stock = stockRepo.GetByProductId(total.ProductId);
                    if (stock == null)
                    {
                        stockRepo.Add(new Stock
                        {
                            ProductId = total.ProductId,
                            Quantity = total.Total
                        }, saveChanges: false);
                    }
                    else
                    {
                        stock.Quantity += total.Total;
                    }
                }

                foreach (var item in details)
                {
                    transactionRepo.Add(new InventoryTransaction
                    {
                        ProductId = item.ProductId,
                        QuantityDelta = item.Quantity,
                        Type = "Import",
                        ReferenceId = item.OrderId,
                        Notes = "Imported from order",
                        CreatedAt = System.DateTime.Now
                    }, saveChanges: false);
                }

                var orderIds = details.Select(x => x.OrderId).Distinct().ToList();
                orderRepo.SetStatusForOrders(orderIds, "Imported", saveChanges: false);

                stockRepo.SaveChanges();
                transactionRepo.SaveChanges();
                orderRepo.SaveChanges();
            }
            // Notify stock changes for all affected products for alerts
            foreach (var t in totals) {
                StockChangeNotifier.Notify(t.ProductId);
            }

            return true;
        }

        public List<SupplierImportSummary> FilterSummaries(List<SupplierImportSummary> source, string supplierFilter, string productFilter, int? minAmount, int? maxAmount)
        {
            if (source == null || source.Count == 0)
            {
                return new List<SupplierImportSummary>();
            }

            supplierFilter = (supplierFilter ?? string.Empty).Trim().ToLowerInvariant();
            productFilter = (productFilter ?? string.Empty).Trim().ToLowerInvariant();

            return source
                .Where(s => string.IsNullOrEmpty(supplierFilter) ||
                            (!string.IsNullOrEmpty(s.SupplierName) &&
                             s.SupplierName.ToLowerInvariant().Contains(supplierFilter)))
                .Select(s => new SupplierImportSummary
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName,
                    Products = s.Products
                        .Where(p =>
                            (string.IsNullOrEmpty(productFilter) ||
                             (!string.IsNullOrEmpty(p.ProductName) &&
                              p.ProductName.ToLowerInvariant().Contains(productFilter))) &&
                            (!minAmount.HasValue || p.TotalQuantity >= minAmount.Value) &&
                            (!maxAmount.HasValue || p.TotalQuantity <= maxAmount.Value))
                        .ToList()
                })
                .Where(s => s.Products.Count > 0)
                .ToList();
        }
    }

    public class SupplierImportSummary
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<ProductImportSummary> Products { get; set; }
    }

    public class ProductImportSummary
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class ImportItemDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string OrderStatus { get; set; }
        public System.DateTime OrderCreatedAt { get; set; }
        public System.DateTime? OrderReceivedAt { get; set; }
    }
}
