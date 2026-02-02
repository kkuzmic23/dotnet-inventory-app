using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace WPFLayer {
    public class GlobalStockAlertListener {
        private readonly AlertsService _alertsService = new AlertsService();
        private readonly EmailNotificationService _emailService = new EmailNotificationService();

        private readonly HashSet<int> _warningNotified = new HashSet<int>();
        private readonly HashSet<int> _criticalNotified = new HashSet<int>();
        private readonly Dictionary<int, DateTime> _lastEmailByProductId = new Dictionary<int, DateTime>();

        public void Start() => StockChangeNotifier.StockChanged += OnStockChanged;
        public void Stop() => StockChangeNotifier.StockChanged -= OnStockChanged;

        private void OnStockChanged(int productId) {
            Application.Current.Dispatcher.Invoke(() => {
                var p = _alertsService.GetProductWithStockAndLastRestock(productId);
                if (p == null || !p.IsActive) return;

                int qty = p.CurrentQuantity;

                bool isCritical = (qty == 0);
                bool isWarning = (qty > 0 && qty < p.ReorderLevel);
                bool isOk = (qty >= p.ReorderLevel);

                if (isCritical && !_criticalNotified.Contains(productId)) {
                    Notifications.ShowLowStock(p); // CRITICAL -> BalloonIcon.Error
                    _criticalNotified.Add(productId);
                    _warningNotified.Remove(productId);

                    TrySendEmail(p, isCritical: true);
                } else if (isWarning && !_warningNotified.Contains(productId)) {
                    Notifications.ShowLowStock(p); // WARNING -> BalloonIcon.Warning
                    _warningNotified.Add(productId);

                    TrySendEmail(p, isCritical: false);
                } else if (isOk) {
                    _warningNotified.Remove(productId);
                    _criticalNotified.Remove(productId);
                }
            });
        }

        private void TrySendEmail(EntityLayer.Entities.Product p, bool isCritical) {
            if (!CanSendEmail(p.Id)) return;

            _lastEmailByProductId[p.Id] = DateTime.Now;

            _ = _emailService.SendLowStockEmailAsync(p, isCritical);
        }

        private bool CanSendEmail(int productId) {
            int cooldownMin = 30;
            int.TryParse(ConfigurationManager.AppSettings["AlertEmailCooldownMinutes"], out cooldownMin);
            if (cooldownMin <= 0) cooldownMin = 30;

            if (!_lastEmailByProductId.TryGetValue(productId, out var last))
                return true;

            return (DateTime.Now - last) >= TimeSpan.FromMinutes(cooldownMin);
        }
    }
}
