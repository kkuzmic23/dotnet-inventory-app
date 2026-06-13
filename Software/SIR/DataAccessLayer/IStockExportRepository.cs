using EntityLayer.Entities;
using System;
using System.Linq;

namespace DataAccessLayer
{
    public interface IStockExportRepository : IDisposable
    {
        IQueryable<StockExport> GetAll();
        int Add(StockExport export, bool saveChanges = true);
        int Update(StockExport export, bool saveChanges = true);
        int SaveChanges();
    }
}
