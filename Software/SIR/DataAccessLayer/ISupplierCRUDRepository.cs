using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ISupplierCRUDRepository : IDisposable
    {
        IQueryable<Supplier> GetAll();

        int Add(Supplier supplier, bool saveChanges = true);

        int Update(Supplier supplier, bool saveChanges = true);

        int Remove(Supplier supplier, bool saveChanges = true);

        int GetProductCount(Supplier supplier);
    }
}
