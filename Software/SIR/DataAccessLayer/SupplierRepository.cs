using EntityLayer.Entities;
using System.Linq;

namespace DataAccessLayer
{
    public class SupplierRepository : Repository<Supplier>, ISupplierCRUDRepository
    {
        public SupplierRepository() : base(new Model1())
        {
        }

        public override int Update(Supplier supplier, bool saveChanges = true)
        {
            var existing = Entities.SingleOrDefault(s => s.Id == supplier.Id);
            if (existing == null)
            {
                return 0;
            }

            existing.Name = supplier.Name;
            existing.Email = supplier.Email;
            existing.Phone = supplier.Phone;
            existing.Address = supplier.Address;
            existing.CreatedAt = supplier.CreatedAt;

            return saveChanges ? SaveChanges() : 0;
        }

        public int GetProductCount(Supplier supplier)
        {
            return Entities
                .Where(s => s.Id == supplier.Id)
                .Select(s => s.Products.Count)
                .SingleOrDefault();
        }
    }
}
