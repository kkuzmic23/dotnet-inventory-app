using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace WPFLayer
{
    internal static class HelpService
    {
        private const string ManualFileName = "User Manual.pdf";
        private const int DefaultPage = 5;
        private static readonly Dictionary<Type, int> PageByType = new Dictionary<Type, int>
        {
            //Page-ovi ne rade u edge-u ali valjda su ispravne
            { typeof(LoginWindow), 4 },
            { typeof(MainWindow), 5 },
            { typeof(OrdersControl), 6 },
            { typeof(OrderModal), 7 },
            { typeof(ProductsControl), 8 },
            { typeof(ProductModal), 9 },
            { typeof(SuppliersControl), 10 },
            { typeof(SupplierModal), 11 }
        };

        public static void ShowHelpForContext(object context)
        {
            var page = GetPageForContext(context);
            OpenManualAtPage(page);
        }

        private static int GetPageForContext(object context)
        {
            if (context == null)
            {
                return DefaultPage;
            }

            var contextType = context.GetType();
            if (PageByType.TryGetValue(contextType, out var page))
            {
                return page;
            }

            return DefaultPage;
        }

        private static void OpenManualAtPage(int page)
        {
            var manualPath = FindManualPath();
            if (!File.Exists(manualPath))
            {
                MessageBox.Show($"Manual not found at: {manualPath}");
                return;
            }

            var uri = new Uri(manualPath, UriKind.Absolute);
            var target = new UriBuilder(uri) { Fragment = $"page={page}" }.Uri.AbsoluteUri;

            Process.Start(new ProcessStartInfo
            {
                FileName = target,
                UseShellExecute = true
            });
        }

        private static string FindManualPath()
        {
            var current = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            for (var i = 0; i < 6 && current != null; i++)
            {
                var candidate = Path.Combine(current.FullName, ManualFileName);
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ManualFileName);
        }
    }
}
