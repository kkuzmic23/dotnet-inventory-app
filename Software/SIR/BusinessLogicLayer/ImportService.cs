using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class ImportService
    {
        public List<OrderHasProduct> GetImports()
        {
            using (var repo = new OrderHasProductRepository())
            {
                return repo.GetAll().ToList();
            }
        }
    }
}
