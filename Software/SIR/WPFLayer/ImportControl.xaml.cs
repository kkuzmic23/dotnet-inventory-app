using System;
using BusinessLogicLayer;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFLayer
{
    /// <summary>
    /// Interaction logic for ImportControl.xaml
    /// </summary>
    public partial class ImportControl : UserControl
    {
        private List<SupplierImportSummary> _allSummaries = new List<SupplierImportSummary>();

        public ImportControl()
        {
            InitializeComponent();
        }

        private void btnLoadImports_Click(object sender, RoutedEventArgs e)
        {
            var service = new ImportService();
            _allSummaries = service.GetImportSummaries();
            lvImports.ItemsSource = _allSummaries;
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (lvImports.SelectedItem is SupplierImportSummary summary)
            {
                var service = new ImportService();
                var details = service.GetImportDetailsBySupplier(summary.SupplierId);
                var window = new ImportDetailsWindow(summary, details);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a supplier import to view details.");
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            lvImports.ItemsSource = ApplyFilters(_allSummaries);
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            txtSupplierFilter.Text = string.Empty;
            txtProductFilter.Text = string.Empty;
            txtAmountMin.Text = string.Empty;
            txtAmountMax.Text = string.Empty;
            lvImports.ItemsSource = _allSummaries;
        }

        private List<SupplierImportSummary> ApplyFilters(List<SupplierImportSummary> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<SupplierImportSummary>();
            }

            string supplierFilter = (txtSupplierFilter.Text ?? string.Empty).Trim().ToLowerInvariant();
            string productFilter = (txtProductFilter.Text ?? string.Empty).Trim().ToLowerInvariant();

            int? minAmount = TryParseAmount(txtAmountMin.Text);
            int? maxAmount = TryParseAmount(txtAmountMax.Text);

            var filtered = source
                .Where(s => string.IsNullOrEmpty(supplierFilter) ||
                            (!string.IsNullOrEmpty(s.SupplierName) &&
                             s.SupplierName.ToLowerInvariant().Contains(supplierFilter)))
                .Select(s => new SupplierImportSummary
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName,
                    Products = s.Products
                        .Where(p =>
                            (string.IsNullOrEmpty(productFilter) ||
                             (!string.IsNullOrEmpty(p.ProductName) &&
                              p.ProductName.ToLowerInvariant().Contains(productFilter))) &&
                            (!minAmount.HasValue || p.TotalQuantity >= minAmount.Value) &&
                            (!maxAmount.HasValue || p.TotalQuantity <= maxAmount.Value))
                        .ToList()
                })
                .Where(s => s.Products.Count > 0)
                .ToList();

            return filtered;
        }

        private int? TryParseAmount(string text)
        {
            if (int.TryParse((text ?? string.Empty).Trim(), out int value))
            {
                return value;
            }

            return null;
        }
    }
}
