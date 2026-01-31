using BusinessLogicLayer;
using System.Windows;
using System.Windows.Controls;

namespace WPFLayer
{
    public partial class StockControl : UserControl
    {
        public StockControl()
        {
            InitializeComponent();
        }

        private void btnLoadStock_Click(object sender, RoutedEventArgs e)
        {
            var service = new StockService();
            dgStock.ItemsSource = service.GetStock();
        }
    }
}
