using EntityLayer.Entities;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository() : base(new Model1())
        {
        }

        public override IQueryable<Product> GetAll()
        {
            return Entities.Include(p => p.Supplier);
        }

        public override int Update(Product product, bool saveChanges = true)
        {
            var existing = Entities.SingleOrDefault(p => p.Id == product.Id);
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

            return saveChanges ? SaveChanges() : 0;
        }
    }
}
