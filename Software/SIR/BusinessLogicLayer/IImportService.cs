using EntityLayer.Entities;
using System.Collections.Generic;

namespace BusinessLogicLayer
{
    public interface IImportService
    {
        List<OrderHasProduct> GetImports();
        List<SupplierImportSummary> GetImportSummaries();
        List<ImportItemDetail> GetImportDetailsBySupplier(int supplierId);
        bool ApplyImport(int supplierId);
        List<SupplierImportSummary> FilterSummaries(List<SupplierImportSummary> source, string supplierFilter, string productFilter, int? minAmount, int? maxAmount);
    }
}
