using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SupplierRepository : Repository<Supplier>
    {
        public SupplierRepository() : base(new Model1())
        {

        }

        public override int Update(Supplier entity, bool saveChanges = true)
        {
            var supplier = Entities.SingleOrDefault(s => s.Id == entity.Id);

            supplier.Name = entity.Name;
            supplier.Email = entity.Email;
            supplier.Phone = entity.Phone;
            supplier.Address = entity.Address;
            supplier.CreatedAt = entity.CreatedAt;

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public IQueryable<Supplier> GetSuppliersByName(string phrase)
        {
            var query = from s in Entities
                        where s.Name.Contains(phrase)
                        select s;

            return query;
        }

        public IQueryable<int> GetProductCount(Supplier supplier)
        {
            var query = from s in Entities
                        where s.Id == supplier.Id
                        select s.Products.Count;
            return query;
        }
    }
}
