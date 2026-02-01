using EntityLayer.Entities;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFLayer {
    public static class Notifications {
    public static void ShowLowStock(Product p) {
            var tray = Application.Current.Resources["TrayIcon"] as TaskbarIcon;
            if (tray == null) return;

            if (tray.IconSource == null) {
                tray.IconSource = GetDefaultIconSource(); // ono što si već imao
                tray.Visibility = Visibility.Visible;
            }

            var title = p.IsCriticalStock ? "CRITICAL: Low stock" : "Warning: Low stock";
            var msg = $"{p.Name}\nQty {p.CurrentQuantity} / Min {p.ReorderLevel} (missing {p.MissingToMinimum})";

            var icon = p.IsCriticalStock ? BalloonIcon.Error : BalloonIcon.Warning;
            tray.ShowBalloonTip(title, msg, icon);
        }


    private static ImageSource GetDefaultIconSource() {
            var icon = SystemIcons.Information;
            return Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
