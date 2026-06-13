using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class ExportService : IExportService
    {
        private readonly IProductCRUDRepository productRepo;
        private readonly IStockExportRepository exportRepo;
        private readonly IStockExportHasProductRepository exportItemRepo;
        private readonly IInventoryTransactionRepository transactionRepo;
        private readonly IStockRepository stockRepo;

        public ExportService() : this(new ProductRepository(), new StockExportRepository(), new StockExportHasProductRepository(), new InventoryTransactionRepository(), new StockRepository())
        {
        }

        public ExportService(IProductCRUDRepository productRepo, IStockExportRepository exportRepo, IStockExportHasProductRepository exportItemRepo, IInventoryTransactionRepository transactionRepo, IStockRepository stockRepo)
        {
            this.productRepo = productRepo;
            this.exportRepo = exportRepo;
            this.exportItemRepo = exportItemRepo;
            this.transactionRepo = transactionRepo;
            this.stockRepo = stockRepo;
        }

        public List<Product> GetProducts()
        {
            return productRepo.GetAll().ToList();
        }

        public List<StockExport> GetExports()
        {
            return exportRepo.GetAll().OrderByDescending(e => e.CreatedAt).ToList();
        }

        public ExportResult Export(ExportRequest request)
        {
            var validationResult = ValidateExportRequest(request);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            var groupedItems = GroupExportItems(request.Items);

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

        private Dictionary<int, Stock> GetStockForProducts(IStockRepository stockRepo, IEnumerable<int> productIds)
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

                if (!stockByProduct.TryGetValue((int)item.ProductId, out Stock stock))
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

        private StockExport CreateExportRecord(IStockExportRepository exportRepo, string notes)
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

        private void ProcessExportItems(List<dynamic> groupedItems, int exportId, Dictionary<int, Stock> stockByProduct, IStockExportHasProductRepository exportItemRepo, IInventoryTransactionRepository transactionRepo)
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
