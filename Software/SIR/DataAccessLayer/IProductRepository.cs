using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer {
    public interface IProductRepository {
        List<Product> GetAll();
        Product GetById(int id);
        List<Product> GetAllWithStock();
        List<Product> GetLowStockProducts();
    }
}
