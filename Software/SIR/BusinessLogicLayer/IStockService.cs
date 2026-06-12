using EntityLayer.Entities;
using System.Collections.Generic;

namespace BusinessLogicLayer
{
    public interface IStockService
    {
        List<Stock> GetStock();
        List<Stock> SearchStock(List<Stock> source, string searchTerm);
    }
}
