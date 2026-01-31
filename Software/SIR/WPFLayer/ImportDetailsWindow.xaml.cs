using BusinessLogicLayer;
using System.Collections.Generic;
using System.Windows;

namespace WPFLayer
{
    public partial class ImportDetailsWindow : Window
    {
        private readonly SupplierImportSummary _summary;

        public ImportDetailsWindow(SupplierImportSummary summary, List<ImportItemDetail> details)
        {
            InitializeComponent();

            _summary = summary;
            txtSupplierName.Text = summary?.SupplierName ?? "Supplier";
            icTotals.ItemsSource = summary?.Products ?? new List<ProductImportSummary>();
            dgDetails.ItemsSource = details ?? new List<ImportItemDetail>();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (_summary == null)
            {
                MessageBox.Show("No import selected.");
                return;
            }

            var confirm = MessageBox.Show(
                "Apply this import to stock quantities?",
                "Confirm import",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
            {
                return;
            }

            var service = new ImportService();
            bool ok = service.ApplyImport(_summary);
            MessageBox.Show(ok ? "Import applied." : "Import could not be applied.");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
