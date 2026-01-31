using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository() : base(new Model1())
        {

        }

        public override IQueryable<Product> GetAll()
        {
            var query = from p in Entities.Include("Supplier")
                        select p;
            return query;
        }

        public IQueryable<Product> GetProductsByName(string phrase)
        {
            var query = from p in Entities.Include("Supplier")
                        where p.Name.Contains(phrase)
                        select p;

            return query;
        }

        public override int Update(Product entity, bool saveChanges = true)
        {
            var product = Entities.SingleOrDefault(p => p.Id == entity.Id);

            product.ProductCode = entity.ProductCode;
            product.Name = entity.Name;
            product.Description = entity.Description;
            product.SupplierId = entity.SupplierId;
            product.ReorderLevel = entity.ReorderLevel;
            product.IsActive = entity.IsActive;
            product.CreatedAt = entity.CreatedAt;

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }
    }
}
