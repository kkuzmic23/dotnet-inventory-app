using BusinessLogicLayer;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFLayer {
    public partial class AlertsControl : UserControl {
        private readonly AlertsService _alertsService = new AlertsService();

        private readonly HashSet<int> _lowNotified = new HashSet<int>();
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
            Dispatcher.Invoke(() =>
            {
                var p = _alertsService.GetProductWithStockAndLastRestock(productId);
                if (p == null || !p.IsActive) return;

                bool isLow = p.CurrentQuantity < p.ReorderLevel;
                bool isCritical = p.CurrentQuantity <= 0;
                if (isCritical && !_criticalNotified.Contains(productId)) {
                    Notifications.ShowLowStock(p);
                    _criticalNotified.Add(productId);
                }

                if (!isCritical && isLow && !_lowNotified.Contains(productId)) {
                    Notifications.ShowLowStock(p);
                    _lowNotified.Add(productId);
                }

                if (!isLow) {
                    _lowNotified.Remove(productId);
                    _criticalNotified.Remove(productId);
                }

                RefreshList();
            });
        }

        private void RefreshList() {
            lstAlerts.ItemsSource = _alertsService.GetLowStockProductsWithLastRestock();
        }
    }
}

