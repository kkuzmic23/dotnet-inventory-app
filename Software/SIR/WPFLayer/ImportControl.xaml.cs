using System;
using BusinessLogicLayer;
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
    /// Interaction logic for ImportControl.xaml
    /// </summary>
    public partial class ImportControl : UserControl
    {
        public ImportControl()
        {
            InitializeComponent();
        }

        private void btnLoadImports_Click(object sender, RoutedEventArgs e)
        {
            var service = new ImportService();
            dgImports.ItemsSource = service.GetImports();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
