using DataAccessLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Product> GetProductsByName(string phrase)
        {
            using (var repo = new ProductRepository())
            {
                return repo.GetProductsByName(phrase).ToList();
            }
        }

        public bool UpdateProduct(Product product)
        {
            bool isSuccessful = false;
            if (product == null)
            {
                return false;
            }

            using (var repo = new ProductRepository())
            {
                int affectedRows = repo.Update(product);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public bool RemoveProduct(Product product)
        {
            bool isSuccessful = false;
            if (product == null)
            {
                return false;
            }

            using (var repo = new ProductRepository())
            {
                int affectedRows = repo.Remove(product);
                isSuccessful = affectedRows > 0;
            }

            return isSuccessful;
        }

        public bool AddProduct(Product product)
        {
            bool isSuccessful = false;
            if (product == null)
            {
                return false;
            }

            using (var repo = new ProductRepository())
            {
                int affectedRows = repo.Add(product);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }
    }
}
