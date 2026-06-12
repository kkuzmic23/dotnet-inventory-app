using BusinessLogicLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFLayer
{
    public partial class ExportControl : UserControl
    {
        private readonly ExportService exportService = new ExportService();
        private readonly StockService stockService = new StockService();
        private readonly ObservableCollection<ExportItemRequest> exportItems = new ObservableCollection<ExportItemRequest>();

        public ExportControl()
        {
            InitializeComponent();
            dgExportItems.ItemsSource = exportItems;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, OnHelp));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Help, new KeyGesture(Key.F1)));
            LoadProducts();
            LoadExports();
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

        private void LoadExports()
        {
            dgExports.ItemsSource = exportService.GetExports();
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

            var request = new ExportRequest
            {
                Notes = txtNotes.Text?.Trim(),
                Items = exportItems.ToList()
            };

            var result = exportService.Export(request);
            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage ?? "Export failed.");
                return;
            }

            MessageBox.Show("Export completed.");
            exportItems.Clear();
            txtNotes.Text = string.Empty;
            LoadExports();
        }

        private void OnHelp(object sender, ExecutedRoutedEventArgs e)
        {
            HelpService.ShowHelpForContext(this);
        }
    }
}
