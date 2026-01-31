using DataAccessLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer {
    public class MockProductService : IProductService{
        private readonly IProductRepository _repo;

        public MockProductService() {
            _repo = new MockProductRepository();
        }

        public List<Product> GetLowStockProductsForDisplay() {
            var products = _repo.GetLowStockProducts();

            return products
                .OrderBy(p => p.CurrentQuantity)
                .ToList();
        }
    }
}
