using System;
using System.IO;
using System.Windows;

namespace WPFLayer {
    public partial class ViewLogsWindow : Window {
        public ViewLogsWindow() {
            InitializeComponent();
            LoadLogs();
        }

        private void LoadLogs() {
            txtLogs.Text = Logger.ReadAll();
            txtLogs.CaretIndex = txtLogs.Text.Length;
            txtLogs.ScrollToEnd();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) {
            LoadLogs();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) {
            try {
                var appDir = AppDomain.CurrentDomain.BaseDirectory;
                var logsDir = System.IO.Path.Combine(appDir, "logs");
                var path = System.IO.Path.Combine(logsDir, "app.log");
                if (File.Exists(path)) File.WriteAllText(path, string.Empty);
            } catch { }
            LoadLogs();
        }
    }
}
