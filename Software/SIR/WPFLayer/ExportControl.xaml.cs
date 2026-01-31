using BusinessLogicLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFLayer
{
    public partial class ExportControl : UserControl
    {
        private readonly ExportService exportService = new ExportService();
        private readonly StockService stockService = new StockService();
        private readonly ObservableCollection<ExportItemRequest> exportItems = new ObservableCollection<ExportItemRequest>();
        private Dictionary<int, int> stockByProductId = new Dictionary<int, int>();

        public ExportControl()
        {
            InitializeComponent();
            dgExportItems.ItemsSource = exportItems;
            LoadProducts();
            LoadStock();
        }

        private void LoadProducts()
        {
            var products = exportService.GetProducts();
            cmbProducts.ItemsSource = products;
            if (products.Count > 0)
            {
                cmbProducts.SelectedIndex = 0;
            }
        }

        private void LoadStock()
        {
            var stock = stockService.GetStock();
            stockByProductId = stock.ToDictionary(s => s.ProductId, s => s.Quantity);
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(cmbProducts.SelectedItem is Product product))
            {
                MessageBox.Show("Select a product.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text?.Trim(), out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Enter a valid quantity.");
                return;
            }

            exportItems.Add(new ExportItemRequest
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = quantity
            });

            txtQuantity.Text = string.Empty;
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgExportItems.SelectedItem is ExportItemRequest item)
            {
                exportItems.Remove(item);
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (exportItems.Count == 0)
            {
                MessageBox.Show("Add at least one item to export.");
                return;
            }

            var grouped = exportItems
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().ProductName,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            foreach (var item in grouped)
            {
                if (!stockByProductId.TryGetValue(item.ProductId, out int available))
                {
                    MessageBox.Show($"Product not in stock: {item.ProductName}.");
                    return;
                }

                if (available < item.Quantity)
                {
                    MessageBox.Show($"Not enough stock for {item.ProductName}. Available: {available}.");
                    return;
                }
            }

            var request = new ExportRequest
            {
                Notes = txtNotes.Text?.Trim(),
                Items = exportItems.ToList()
            };

            bool ok = exportService.Export(request);
            if (!ok)
            {
                MessageBox.Show("Export failed.");
                return;
            }

            MessageBox.Show("Export completed.");
            exportItems.Clear();
            txtNotes.Text = string.Empty;
            LoadStock();
        }
    }
}
