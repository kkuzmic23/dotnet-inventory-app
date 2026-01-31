using BusinessLogicLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for SuppliersControl.xaml
    /// </summary>
    public partial class SuppliersControl : UserControl
    {
        private SupplierService supplierService = new SupplierService();
        private ObservableCollection<Supplier> suppliers = new ObservableCollection<Supplier>();
        public SuppliersControl()
        {
            InitializeComponent();
            dgSupplier.ItemsSource = suppliers;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            suppliers = new ObservableCollection<Supplier>(supplierService.GetSuppliers());
            dgSupplier.ItemsSource = suppliers;
        }

        public void LoadAndSelectSupplier(int id)
        {
            LoadSuppliers();

            var supplier = suppliers.FirstOrDefault(x => x.Id == id);
            if (supplier == null)
            {
                return;
            }

            dgSupplier.SelectedItem = supplier;
            dgSupplier.ScrollIntoView(supplier);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            LoadSuppliers();
        }

        private Supplier GetSelectedSupplier()
        {
            return dgSupplier.SelectedItem as Supplier;
        }

        private void btnAllProductsFromSupplier_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
