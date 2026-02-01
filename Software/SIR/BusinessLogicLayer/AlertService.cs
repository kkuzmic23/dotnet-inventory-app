using DataAccessLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer {
    public class AlertsService {
        public Product GetProductWithStockAndLastRestock(int productId) {
            using (var stockRepo = new StockRepository())
            using (var trxRepo = new InventoryTransactionRepository())
            using (var productRepo = new ProductRepository()) {
                var stock = stockRepo.GetByProductId(productId);

                Product product =
                    (from p in productRepo.GetAll()
                     where p.Id == productId
                     select p).FirstOrDefault();

                if (product == null)
                    return null;

                if (stock != null)
                    product.Stock = stock;

                var lastRestock =
                    (from t in trxRepo.GetAll()
                     where t.ProductId == productId
                     where t.QuantityDelta > 0
                     orderby t.CreatedAt descending
                     select (DateTime?)t.CreatedAt).FirstOrDefault();

                product.LastRestockAt = lastRestock;
                return product;
            }
        }

        public List<Product> GetLowStockProductsWithLastRestock() {
            using (var stockRepo = new StockRepository())
            using (var trxRepo = new InventoryTransactionRepository()) {
                var lowStocks =
                    (from s in stockRepo.GetAll()
                     where s.Product.IsActive
                     where s.Quantity < s.Product.ReorderLevel
                     orderby s.Quantity ascending
                     select s).ToList();

                var lastRestockMap =
                    (from t in trxRepo.GetAll()
                     where t.QuantityDelta > 0
                     group t by t.ProductId into g
                     select new {
                         ProductId = g.Key,
                         LastRestockAt = g.Max(x => x.CreatedAt)
                     })
                    .ToDictionary(x => x.ProductId, x => (DateTime?)x.LastRestockAt);

                var products =
                    (from s in lowStocks
                     select s.Product).ToList();

                foreach (var s in lowStocks) {
                    s.Product.Stock = s;

                    if (lastRestockMap.TryGetValue(s.ProductId, out var dt))
                        s.Product.LastRestockAt = dt;
                    else
                        s.Product.LastRestockAt = null;
                }

                return products;
            }
        }
    }

    public static class StockChangeNotifier {
        public static event Action<int> StockChanged;

        public static void Notify(int productId) {
            StockChanged?.Invoke(productId);
        }
    }
}
