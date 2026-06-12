using DataAccessLayer;
using EntityLayer.Entities;
using System;
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
            if (!ValidateProduct(product).IsSuccessful)
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
            if (!ValidateProduct(product).IsSuccessful)
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

        public bool ToggleProductActiveStatus(Product product)
        {
            if (product == null)
            {
                return false;
            }

            product.IsActive = !product.IsActive;
            return UpdateProduct(product);
        }

        public ServiceResult ValidateProduct(Product product)
        {
            if (product == null)
            {
                return ServiceResult.Failure("Product is required");
            }

            if (string.IsNullOrWhiteSpace(product.ProductCode) || string.IsNullOrWhiteSpace(product.Name))
            {
                return ServiceResult.Failure("Name and product code are required");
            }

            if (product.ReorderLevel < 0)
            {
                return ServiceResult.Failure("Reorder level must be positive number");
            }

            return ServiceResult.Success();
        }

        public List<Product> FilterProducts(IEnumerable<Product> products, int supplierId, string searchPhrase)
        {
            if (products == null)
            {
                return new List<Product>();
            }

            IEnumerable<Product> filtered = products;

            if (supplierId != 0)
            {
                filtered = filtered.Where(x => x.SupplierId == supplierId);
            }

            string phrase = searchPhrase?.Trim();
            if (!string.IsNullOrWhiteSpace(phrase))
            {
                filtered = filtered.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.Name) &&
                     x.Name.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0)
                    ||
                    (!string.IsNullOrWhiteSpace(x.ProductCode) &&
                     x.ProductCode.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }

            return filtered.ToList();
        }
    }
}
