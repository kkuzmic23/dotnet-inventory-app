using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class ExportService
    {
        public List<Product> GetProducts()
        {
            using (var repo = new ProductRepository())
            {
                return repo.GetAll().ToList();
            }
        }
    }

    public class ExportItemRequest
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

    public class ExportRequest
    {
        public string Notes { get; set; }
        public List<ExportItemRequest> Items { get; set; } = new List<ExportItemRequest>();
    }
}
