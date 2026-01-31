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
        public ProductsControl()
        {
            InitializeComponent();
            dgProducts.ItemsSource = products;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            products = new ObservableCollection<Product>(productService.GetProducts());
            dgProducts.ItemsSource = products;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var product = GetSelectedProduct();
            if (product == null || product.Id == 0)
            {
                MessageBox.Show("Select a product first");
                return;
            }

            bool isSuccessful = productService.RemoveProduct(product);
            if (!isSuccessful)
            {
                MessageBox.Show("Fatal failure while deleting product");
            }

            LoadProducts();
        }

        private Product GetSelectedProduct()
        {
            return dgProducts.SelectedItem as Product;
        }
    }
}
