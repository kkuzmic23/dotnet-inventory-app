using EntityLayer.Entities;
using System;

namespace DataAccessLayer
{
    public interface IStockExportHasProductRepository : IDisposable
    {
        int Add(StockExportHasProduct item, bool saveChanges = true);
        int SaveChanges();
    }
}
