using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class ProductRepository
    {
        public List<Product> GetAll()
        {
            using (var context = new Model1())
            {
                return context.Products.ToList();
            }
        }

        public int Add(Product product)
        {
            using (var context = new Model1())
            {
                context.Products.Add(product);
                return context.SaveChanges();
            }
        }

        public int Update(Product product)
        {
            using (var context = new Model1())
            {
                var existing = context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (existing == null)
                {
                    return 0;
                }

                existing.ProductCode = product.ProductCode;
                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.SupplierId = product.SupplierId;
                existing.ReorderLevel = product.ReorderLevel;
                existing.IsActive = product.IsActive;
                existing.CreatedAt = product.CreatedAt;

                return context.SaveChanges();
            }
        }

        public int Remove(Product product)
        {
            using (var context = new Model1())
            {
                var existing = context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (existing == null)
                {
                    return 0;
                }

                context.Products.Remove(existing);
                return context.SaveChanges();
            }
        }
    }
}
