using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class SupplierRepository
    {
        public List<Supplier> GetAll()
        {
            using (var context = new Model1())
            {
                return context.Suppliers.ToList();
            }
        }

        public int Add(Supplier supplier)
        {
            using (var context = new Model1())
            {
                context.Suppliers.Add(supplier);
                return context.SaveChanges();
            }
        }

        public int Update(Supplier supplier)
        {
            using (var context = new Model1())
            {
                var existing = context.Suppliers.FirstOrDefault(s => s.Id == supplier.Id);
                if (existing == null)
                {
                    return 0;
                }

                existing.Name = supplier.Name;
                existing.Email = supplier.Email;
                existing.Phone = supplier.Phone;
                existing.Address = supplier.Address;
                existing.CreatedAt = supplier.CreatedAt;

                return context.SaveChanges();
            }
        }

        public int Remove(Supplier supplier)
        {
            using (var context = new Model1())
            {
                var existing = context.Suppliers.FirstOrDefault(s => s.Id == supplier.Id);
                if (existing == null)
                {
                    return 0;
                }

                context.Suppliers.Remove(existing);
                return context.SaveChanges();
            }
        }
    }
}
