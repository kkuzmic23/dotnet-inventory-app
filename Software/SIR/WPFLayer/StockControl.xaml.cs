using BusinessLogicLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFLayer
{
    public partial class StockControl : UserControl
    {
        private List<Stock> _allStock = new List<Stock>();

        public StockControl()
        {
            InitializeComponent();
        }

        private void btnLoadStock_Click(object sender, RoutedEventArgs e)
        {
            var service = new StockService();
            _allStock = service.GetStock();
            dgStock.ItemsSource = _allStock;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgStock.ItemsSource = ApplySearch(_allStock);
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = string.Empty;
            dgStock.ItemsSource = _allStock;
        }

        private List<Stock> ApplySearch(List<Stock> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<Stock>();
            }

            string term = (txtSearch.Text ?? string.Empty).Trim().ToLowerInvariant();
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
