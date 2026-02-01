using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;

namespace DataAccessLayer {
    public class MockProductRepository : IProductRepository {
        private readonly List<Product> _products;
        private readonly List<Stock> _stocks;

        public MockProductRepository() {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Milk 1L", ReorderLevel = 10, IsActive = true },
                new Product { Id = 2, Name = "White Bread", ReorderLevel = 15, IsActive = true },
                new Product { Id = 3, Name = "Cheddar Cheese 500g", ReorderLevel = 8, IsActive = true },
                new Product { Id = 4, Name = "Eggs 12-pack", ReorderLevel = 8, IsActive = true },
                new Product { Id = 5, Name = "Rice 5kg", ReorderLevel = 5, IsActive = true },
                new Product { Id = 6, Name = "Pasta 1kg", ReorderLevel = 6, IsActive = true },
                new Product { Id = 7, Name = "Apples 1kg", ReorderLevel = 12, IsActive = true },
                new Product { Id = 8, Name = "Bananas 1kg", ReorderLevel = 12, IsActive = true },
                new Product { Id = 9, Name = "Onions 2kg", ReorderLevel = 7, IsActive = true },
                new Product { Id = 10, Name = "Flour 5kg", ReorderLevel = 5, IsActive = true },
                new Product { Id = 11, Name = "Sugar 2kg", ReorderLevel = 6, IsActive = true },
                new Product { Id = 13, Name = "Potpisi", ReorderLevel = 7, IsActive = true },
                new Product { Id = 12, Name = "Cooking Oil 1L", ReorderLevel = 4, IsActive = true },

                new Product { Id = 99, Name = "Old Product", ReorderLevel = 10, IsActive = false },
            };
            var now = DateTime.Now;

            foreach (var p in _products) {
                // da nije svima isto
                if (p.Id % 3 == 0) p.LastRestockAt = now.AddHours(-2);
                else if (p.Id % 3 == 1) p.LastRestockAt = now.AddDays(-1);
                else p.LastRestockAt = now.AddDays(-5);

                // za neki proizvod recimo da nikad nije restockan
                if (p.Id == 11) p.LastRestockAt = null;
            }

            _stocks = new List<Stock>
            {
                new Stock { ProductId = 1, Quantity = 47 },
                new Stock { ProductId = 2, Quantity = 47 },
                new Stock { ProductId = 3, Quantity = -4 },
                new Stock { ProductId = 4, Quantity = 24 },
                new Stock { ProductId = 5, Quantity = 20 },
                new Stock { ProductId = 6, Quantity = 25 },
                new Stock { ProductId = 7, Quantity = 36 },
                new Stock { ProductId = 8, Quantity = 40 },
                new Stock { ProductId = 9, Quantity = 30 },
                new Stock { ProductId = 10, Quantity = 0 },
                new Stock { ProductId = 11, Quantity = 0 },
                new Stock { ProductId = 12, Quantity = -2 },
                new Stock { ProductId = 13, Quantity = 6 },

                // namjerno nema Stock za 99 -> CurrentQuantity će biti 0
            };

            // ručno poveži navigation Product.Stock
            foreach (var p in _products) {
                p.Stock = (from s in _stocks
                           where s.ProductId == p.Id
                           select s).FirstOrDefault();
            }
        }

        public List<Product> GetAll() {
            var query =
                from p in _products
                select p;

            return query.ToList();
        }

        public Product GetById(int id) {
            var query =
                from p in _products
                where p.Id == id
                select p;

            return query.FirstOrDefault();
        }

        // repo garantira da je Stock povezan (u mocku je ručno povezan gore)
        public List<Product> GetAllWithStock() {
            var query =
                from p in _products
                where p.IsActive
                orderby p.Name
                select p;

            return query.ToList();
        }

        public List<Product> GetLowStockProducts() {
            var query =
                from p in _products
                where p.IsLowStock
                orderby p.CurrentQuantity ascending
                select p;

            return query.ToList();
        }
    }
}
