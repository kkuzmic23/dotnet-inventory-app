using BusinessLogicLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFLayer
{
    public partial class StockControl : UserControl
    {
        private List<Stock> _allStock = new List<Stock>();

        public StockControl()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, OnHelp));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Help, new KeyGesture(Key.F1)));
        }

        private void btnLoadStock_Click(object sender, RoutedEventArgs e)
        {
            var service = new StockService();
            _allStock = service.GetStock();
            dgStock.ItemsSource = _allStock;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var service = new StockService();
            dgStock.ItemsSource = service.SearchStock(_allStock, txtSearch.Text);
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = string.Empty;
            dgStock.ItemsSource = _allStock;
        }

        private void OnHelp(object sender, ExecutedRoutedEventArgs e)
        {
            HelpService.ShowHelpForContext(this);
        }
    }
}
