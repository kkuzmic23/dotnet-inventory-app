using BusinessLogicLayer;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFLayer {
    public partial class AlertsControl : UserControl {
        private readonly AlertsService _alertsService = new AlertsService();

        // pamti za koje proizvode je već poslan CRITICAL notify
        private readonly HashSet<int> _criticalNotified = new HashSet<int>();

        public AlertsControl() {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            RefreshList();
            StockChangeNotifier.StockChanged += OnStockChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
            StockChangeNotifier.StockChanged -= OnStockChanged;
        }

        private void OnStockChanged(int productId) {
            Dispatcher.Invoke(() => {
                // 1) provjeri samo taj proizvod
                var p = _alertsService.GetProductWithStockAndLastRestock(productId);
                if (p == null) return;

                // 2) critical pravilo
                bool isCritical = p.IsActive && p.CurrentQuantity < 0;

                // 3) notify samo kad prvi put uđe u critical
                if (isCritical && !_criticalNotified.Contains(productId)) {
                    Notifications.ShowLowStock(p); // već imaš warning/error unutra
                    _criticalNotified.Add(productId);
                }

                // 4) ako se oporavio, dopusti budući notify
                if (!isCritical && _criticalNotified.Contains(productId)) {
                    _criticalNotified.Remove(productId);
                }

                // 5) opcionalno: osvježi listu (da se UI ažurira)
                RefreshList();
            });
        }

        private void RefreshList() {
            lstAlerts.ItemsSource = _alertsService.GetLowStockProductsWithLastRestock();
        }
    }
}

