using EntityLayer.Entities;
using System;

namespace DataAccessLayer
{
    public interface IInventoryTransactionRepository : IDisposable
    {
        int Add(InventoryTransaction transaction, bool saveChanges = true);
        int SaveChanges();
    }
}
