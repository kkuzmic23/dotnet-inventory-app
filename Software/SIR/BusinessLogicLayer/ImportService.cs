using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class ImportService
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
                    .Where(x => x.Order?.Supplier != null && x.Product != null)
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
                    .Where(x => x.Order != null && x.Order.SupplierId == supplierId && x.Product != null)
                    .Select(x => new ImportItemDetail
                    {
                        OrderId = x.OrderId,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity
                    })
                    .OrderBy(x => x.OrderId)
                    .ThenBy(x => x.ProductName)
                    .ToList();
            }
        }

        public bool ApplyImport(SupplierImportSummary summary)
        {
            if (summary == null || summary.Products == null || summary.Products.Count == 0)
            {
                return false;
            }

            using (var repo = new StockRepository())
            {
                foreach (var product in summary.Products)
                {
                    var stock = repo.GetByProductId(product.ProductId);
                    if (stock == null)
                    {
                        repo.Add(new Stock
                        {
                            ProductId = product.ProductId,
                            Quantity = product.TotalQuantity
                        }, saveChanges: false);
                    }
                    else
                    {
                        stock.Quantity += product.TotalQuantity;
                    }
                }

                repo.SaveChanges();
            }

            return true;
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
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
