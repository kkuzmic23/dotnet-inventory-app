using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class ProductService
    {
        public List<Product> GetProducts()
        {
            using (var repo = new ProductRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public bool AddProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }

            using (var repo = new ProductRepository())
            {
                int affectedRows = repo.Add(product);
                return affectedRows > 0;
            }
        }

        public bool UpdateProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }

            using (var repo = new ProductRepository())
            {
                int affectedRows = repo.Update(product);
                return affectedRows > 0;
            }
        }

        public bool RemoveProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }

            using (var repo = new ProductRepository())
            {
                int affectedRows = repo.Remove(product);
                return affectedRows > 0;
            }
        }
    }
}