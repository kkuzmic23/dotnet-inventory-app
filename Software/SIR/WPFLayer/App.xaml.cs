using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

// ... existing usings

namespace WPFLayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            // Initialize logger
            Logger.Initialize();

            // Global exception handlers
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            Logger.LogError("DispatcherUnhandledException", e.Exception);
            // Let default behavior continue (optionally set e.Handled = true)
        }

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            if (e.ExceptionObject is Exception ex) {
                Logger.LogError("DomainUnhandledException", ex);
            } else {
                Logger.Log($"DomainUnhandledException: {e.ExceptionObject}");
            }
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e) {
            Logger.LogError("UnobservedTaskException", e.Exception);
            // prevent crashing the process in some scenarios
            e.SetObserved();
        }
    }
}
