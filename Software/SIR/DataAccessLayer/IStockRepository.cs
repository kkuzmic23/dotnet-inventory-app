using EntityLayer.Entities;
using System;
using System.Linq;

namespace DataAccessLayer
{
    public interface IStockRepository : IDisposable
    {
        IQueryable<Stock> GetAll();
        Stock GetByProductId(int productId);
        int Add(Stock stock, bool saveChanges = true);
        int Update(Stock stock, bool saveChanges = true);
        int SaveChanges();
    }
}
