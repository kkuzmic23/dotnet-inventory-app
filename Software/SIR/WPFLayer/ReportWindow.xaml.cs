using EntityLayer.Entities;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WPFLayer {
    public partial class ReportWindow : Window {
        private readonly FlowDocument _doc;
        private readonly StatisticsSummary _summary;
        private readonly DateTime _from;
        private readonly DateTime _to;
        private readonly string _generatedBy;

        public ReportWindow(StatisticsSummary summary, DateTime from, DateTime to, string generatedBy) {
            InitializeComponent();

            _summary = summary;
            _from = from;
            _to = to;
            _generatedBy = generatedBy;

            _doc = ReportBuilder.BuildReportDocument(summary, from, to, generatedBy);
            viewer.Document = _doc;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void BtnSaveXps_Click(object sender, RoutedEventArgs e) {
            var dlg = new SaveFileDialog {
                Filter = "XPS document (*.xps)|*.xps",
                FileName = $"report_{DateTime.Now:yyyyMMdd_HHmm}.xps"
            };

            if (dlg.ShowDialog() != true) return;

            try {
                ReportBuilder.SaveFlowDocumentAsXps(_doc, dlg.FileName);
                MessageBox.Show("Report saved successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex) {
                MessageBox.Show($"Failed to save XPS:\n{ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSaveDocx_Click(object sender, RoutedEventArgs e) {
            var dlg = new SaveFileDialog {
                Filter = "Word document (*.docx)|*.docx",
                FileName = $"report_{DateTime.Now:yyyyMMdd_HHmm}.docx"
            };

            if (dlg.ShowDialog() != true) return;

            try {
                ReportBuilder.SaveFlowDocumentAsDocx(_doc, dlg.FileName);
                MessageBox.Show("Report saved successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex) {
                MessageBox.Show($"Failed to save DOCX:\n\n{ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnPrint_Click(object sender, RoutedEventArgs e) {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() != true) return;

            try {
                ReportBuilder.PrepareDocumentForA4(_doc);
                IDocumentPaginatorSource dps = _doc;
                pd.PrintDocument(dps.DocumentPaginator, "Statistics Report");

                MessageBox.Show("Report sent to printer.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex) {
                MessageBox.Show($"Failed to print:\n{ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}