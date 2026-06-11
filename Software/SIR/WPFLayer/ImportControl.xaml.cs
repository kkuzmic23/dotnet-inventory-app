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
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, OnHelp));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Help, new KeyGesture(Key.F1)));
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
            var service = new ImportService();
            int? minAmount = TryParseAmount(txtAmountMin.Text);
            int? maxAmount = TryParseAmount(txtAmountMax.Text);

            lvImports.ItemsSource = service.FilterSummaries(
                _allSummaries,
                txtSupplierFilter.Text,
                txtProductFilter.Text,
                minAmount,
                maxAmount);
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            txtSupplierFilter.Text = string.Empty;
            txtProductFilter.Text = string.Empty;
            txtAmountMin.Text = string.Empty;
            txtAmountMax.Text = string.Empty;
            lvImports.ItemsSource = _allSummaries;
        }

        private int? TryParseAmount(string text)
        {
            if (int.TryParse((text ?? string.Empty).Trim(), out int value))
            {
                return value;
            }

            return null;
        }

        private void OnHelp(object sender, ExecutedRoutedEventArgs e)
        {
            HelpService.ShowHelpForContext(this);
        }
    }
}
