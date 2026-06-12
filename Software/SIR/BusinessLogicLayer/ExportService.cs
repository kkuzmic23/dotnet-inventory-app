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
            var validationResult = ValidateExportRequest(request);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            var groupedItems = GroupExportItems(request.Items);

            using (var stockRepo = new StockRepository())
            using (var exportRepo = new StockExportRepository())
            using (var exportItemRepo = new StockExportHasProductRepository())
            using (var transactionRepo = new InventoryTransactionRepository())
            {
                var stockByProduct = GetStockForProducts(stockRepo, groupedItems.Select(i => (int)i.ProductId));

                var stockValidation = ValidateStockAvailability(groupedItems, stockByProduct);
                if (!stockValidation.Success)
                {
                    return stockValidation;
                }

                var export = CreateExportRecord(exportRepo, request.Notes);
                ProcessExportItems(groupedItems, export.Id, stockByProduct, exportItemRepo, transactionRepo);

                exportItemRepo.SaveChanges();
                stockRepo.SaveChanges();
                transactionRepo.SaveChanges();

                NotifyStockChanges(groupedItems.Select(x => (int)x.ProductId));
            }

            return new ExportResult { Success = true };
        }

        private ExportResult ValidateExportRequest(ExportRequest request)
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

            return new ExportResult { Success = true };
        }

        private List<dynamic> GroupExportItems(IEnumerable<ExportItemRequest> items)
        {
            return items
                .GroupBy(i => i.ProductId)
                .Select(g => (dynamic)new
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();
        }

        private Dictionary<int, Stock> GetStockForProducts(StockRepository stockRepo, IEnumerable<int> productIds)
        {
            return stockRepo.GetAll()
                .Where(s => productIds.Contains(s.ProductId))
                .ToList()
                .ToDictionary(s => s.ProductId, s => s);
        }

        private ExportResult ValidateStockAvailability(List<dynamic> groupedItems, Dictionary<int, Stock> stockByProduct)
        {
            foreach (var item in groupedItems)
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

            return new ExportResult { Success = true };
        }

        private StockExport CreateExportRecord(StockExportRepository exportRepo, string notes)
        {
            var export = new StockExport
            {
                CreatedAt = System.DateTime.Now,
                Notes = notes
            };

            exportRepo.Add(export, saveChanges: false);
            exportRepo.SaveChanges();
            return export;
        }

        private void ProcessExportItems(List<dynamic> groupedItems, int exportId, Dictionary<int, Stock> stockByProduct, StockExportHasProductRepository exportItemRepo, InventoryTransactionRepository transactionRepo)
        {
            foreach (var item in groupedItems)
            {
                exportItemRepo.Add(new StockExportHasProduct
                {
                    StockExportId = exportId,
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
                    ReferenceId = exportId,
                    Notes = "Exported from stock",
                    CreatedAt = System.DateTime.Now
                }, saveChanges: false);
            }
        }

        private void NotifyStockChanges(IEnumerable<int> productIds)
        {
            foreach (var productId in productIds.Distinct())
            {
                StockChangeNotifier.Notify(productId);
            }
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
