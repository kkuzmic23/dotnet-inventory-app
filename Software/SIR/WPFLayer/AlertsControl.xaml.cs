using BusinessLogicLayer;
using System.Windows;
using System.Windows.Controls;

namespace WPFLayer {
    public partial class AlertsControl : UserControl {
        private readonly IProductService _productService;

        public AlertsControl() {
            InitializeComponent();
            _productService = new MockProductService(); // zasad mock; kasnije pravi service
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            lstAlerts.ItemsSource = _productService.GetLowStockProductsForDisplay();
        }
    }
}
