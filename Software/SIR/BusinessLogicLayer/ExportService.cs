using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class ExportService : IExportService
    {
        public List<Product> GetProducts()
        {
            using (var repo = new ProductRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public List<StockExport> GetExports()
        {
            using (var repo = new StockExportRepository())
            {
                return repo.GetAll().OrderByDescending(e => e.CreatedAt).ToList();
            }
        }

        public ExportResult Export(ExportRequest request)
        {
            if (request == null || request.Items == null || request.Items.Count == 0)
            {
                return new ExportResult { Success = false, ErrorMessage = "Add at least one item to export." };
            }

            var items = request.Items.Where(i => i != null).ToList();
            if (items.Count == 0)
            {
                return new ExportResult { Success = false, ErrorMessage = "Add at least one item to export." };
            }

            var grouped = items
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            using (var stockRepo = new StockRepository())
            using (var exportRepo = new StockExportRepository())
            using (var exportItemRepo = new StockExportHasProductRepository())
            using (var transactionRepo = new InventoryTransactionRepository())
            {
                var stockByProduct = stockRepo.GetAll().ToList()
                    .ToDictionary(s => s.ProductId, s => s);

                foreach (var item in grouped)
                {
                    if (item.ProductId <= 0)
                    {
                        return new ExportResult { Success = false, ErrorMessage = "Invalid product." };
                    }

                    if (item.Quantity <= 0)
                    {
                        return new ExportResult { Success = false, ErrorMessage = "Enter a valid quantity." };
                    }

                    if (!stockByProduct.TryGetValue(item.ProductId, out var stock))
                    {
                        return new ExportResult { Success = false, ErrorMessage = "Product not in stock." };
                    }

                    if (stock.Quantity < item.Quantity)
                    {
                        return new ExportResult { Success = false, ErrorMessage = "Not enough stock." };
                    }
                }

                var export = new StockExport
                {
                    CreatedAt = System.DateTime.Now,
                    Notes = request.Notes
                };

                exportRepo.Add(export, saveChanges: false);
                exportRepo.SaveChanges();

                foreach (var item in grouped)
                {
                    exportItemRepo.Add(new StockExportHasProduct
                    {
                        StockExportId = export.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    }, saveChanges: false);

                    var stock = stockByProduct[item.ProductId];
                    stock.Quantity -= item.Quantity;

                    transactionRepo.Add(new InventoryTransaction
                    {
                        ProductId = item.ProductId,
                        QuantityDelta = -item.Quantity,
                        Type = "Export",
                        ReferenceId = export.Id,
                        Notes = "Exported from stock",
                        CreatedAt = System.DateTime.Now
                    }, saveChanges: false);
                }

                exportItemRepo.SaveChanges();
                stockRepo.SaveChanges();
                transactionRepo.SaveChanges();
                foreach (var productId in grouped.Select(x => x.ProductId).Distinct()) {
                    StockChangeNotifier.Notify(productId);
                }
            }

            return new ExportResult { Success = true };
        }
    }

    public class ExportResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ExportItemRequest
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

    public class ExportRequest
    {
        public string Notes { get; set; }
        public List<ExportItemRequest> Items { get; set; } = new List<ExportItemRequest>();
    }
}
