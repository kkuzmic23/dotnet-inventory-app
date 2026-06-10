using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class StockService
    {
        public List<Stock> GetStock()
        {
            using (var repo = new StockRepository())
            {
                return repo.GetAll().ToList();
            }
        }
    }
}
