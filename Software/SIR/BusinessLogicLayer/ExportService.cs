using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class ExportService
    {
        public List<Product> GetProducts()
        {
            using (var repo = new ProductRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public bool Export(ExportRequest request)
        {
            if (request == null || request.Items == null || request.Items.Count == 0)
            {
                return false;
            }

            var items = request.Items.Where(i => i != null).ToList();
            if (items.Count == 0)
            {
                return false;
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
                        return false;
                    }

                    if (item.Quantity <= 0)
                    {
                        return false;
                    }

                    if (!stockByProduct.TryGetValue(item.ProductId, out var stock))
                    {
                        return false;
                    }

                    if (stock.Quantity < item.Quantity)
                    {
                        return false;
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
            }

            return true;
        }
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
