using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IProductCRUDRepository : IDisposable
    {
        IQueryable<Product> GetAll();

        int Add(Product product, bool saveChanges = true);

        int Update(Product product, bool saveChanges = true);

        int Remove(Product product, bool saveChanges = true);
    }
}
