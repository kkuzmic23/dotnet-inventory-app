using EntityLayer.Entities;
using System.Collections.Generic;

namespace BusinessLogicLayer
{
    public interface IExportService
    {
        List<Product> GetProducts();
        List<StockExport> GetExports();
        ExportResult Export(ExportRequest request);
    }
}
