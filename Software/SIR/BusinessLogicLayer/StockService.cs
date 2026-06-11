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

        public List<Stock> SearchStock(List<Stock> source, string searchTerm)
        {
            if (source == null || source.Count == 0)
            {
                return new List<Stock>();
            }

            string term = (searchTerm ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(term))
            {
                return source;
            }

            return source
                .Where(s =>
                    (s.Product?.Name ?? string.Empty).ToLowerInvariant().Contains(term) ||
                    (s.Product?.ProductCode ?? string.Empty).ToLowerInvariant().Contains(term) ||
                    (s.Product?.Description ?? string.Empty).ToLowerInvariant().Contains(term) ||
                    (s.Product?.Supplier?.Name ?? string.Empty).ToLowerInvariant().Contains(term))
                .ToList();
        }
    }
}
