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
    /// Interaction logic for ProductsControl.xaml
    /// </summary>
    public partial class ProductsControl : UserControl
    {
        private readonly ProductService productService = new ProductService();
        private ObservableCollection<Product> products = new ObservableCollection<Product>();

        private readonly SupplierService supplierService = new SupplierService();
        private List<Product> allProducts = new List<Product>();
        public ProductsControl()
        {
            InitializeComponent();
            dgProducts.ItemsSource = products;
            LoadSuppliers();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            allProducts = productService.GetProducts();
            products = new ObservableCollection<Product>(productService.GetProducts());
            dgProducts.ItemsSource = products;
        }

        private void LoadSuppliers()
        {
            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 0, Name = "All suppliers"}
            };

            suppliers.AddRange(supplierService.GetSuppliers());

            cmbFilter.DisplayMemberPath = "Name";
            cmbFilter.SelectedValuePath = "Id";
            cmbFilter.ItemsSource = suppliers;
            cmbFilter.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
        }

        private Product GetSelectedProduct()
        {
            return dgProducts.SelectedItem as Product;
        }

        private void ApplyFilter()
        {
            if (cmbFilter == null)
            {
                return;
            }

            var selectedSupplier = cmbFilter.SelectedItem as Supplier;
            if (selectedSupplier == null || selectedSupplier.Id == 0)
            {
                products = new ObservableCollection<Product>(allProducts);
            }
            else
            {
                products = new ObservableCollection<Product>(allProducts.Where(p => p.SupplierId == selectedSupplier.Id));
            }

            dgProducts.ItemsSource = products;
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void btnFindSupplier_Click(object sender, RoutedEventArgs e)
        {
            var product = GetSelectedProduct();
            if (product == null)
            {
                MessageBox.Show("Select a product first");
                return;
            }

            int supplierId = 0;
            if (product.SupplierId.HasValue)
            {
                supplierId = product.SupplierId.Value;
            }
            else if (product.Supplier != null)
            {
                supplierId = product.Supplier.Id;
            }

            if (supplierId == 0)
            {
                MessageBox.Show("Selected product somehow has no supplier");
                return;
            }

            var suppliersControl = new SuppliersControl();
            suppliersControl.LoadAndSelectSupplier(supplierId);

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow == null)
            {
                MessageBox.Show("Main window not found");
                return;
            }

            mainWindow.Sadrzaj.Content = suppliersControl;
            mainWindow.lblWelcome.Content = "Suppliers";
        }
    }
}
