using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GlobalStockAlertListener _listener = new GlobalStockAlertListener();
        public MainWindow()
        {
            InitializeComponent();
            _listener.Start();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, OnHelp));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Help, new KeyGesture(Key.F1)));
        }
        protected override void OnClosed(EventArgs e) {
            _listener.Stop();
            base.OnClosed(e);
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            Sadrzaj.Content = new OrdersControl();
            lblWelcome.Content = "Orders";
        }

        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            Sadrzaj.Content = new ProductsControl();
            lblWelcome.Content = "Products";
        }

        private void btnSuppliers_Click(object sender, RoutedEventArgs e)
        {
            Sadrzaj.Content = new SuppliersControl();
            lblWelcome.Content = "Suppliers";
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            Sadrzaj.Content = new ImportControl();
            lblWelcome.Content = "Import";
        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            Sadrzaj.Content = new StockControl();
            lblWelcome.Content = "Stock";
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Sadrzaj.Content = new ExportControl();
            lblWelcome.Content = "Export";
        }

        private void BtnAlerts_Click(object sender, RoutedEventArgs e)
        {
            Sadrzaj.Content = new AlertsControl();
            lblWelcome.Content = "Alerts";
        }

        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnHelp(object sender, ExecutedRoutedEventArgs e)
        {
            HelpService.ShowHelpForContext(Sadrzaj.Content ?? this);
        }
    }
}
