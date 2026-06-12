using BusinessLogicLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WPFLayer
{
    public partial class ProductModal : Window
    {
        private readonly ProductService productService = new ProductService();
        private readonly SupplierService supplierService = new SupplierService();
        private readonly Product existingProduct;
        private List<Supplier> suppliers = new List<Supplier>();
        public ProductModal()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, OnHelp));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Help, new KeyGesture(Key.F1)));
            LoadSuppliers();
            txtCreatedAt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            chkIsActive.IsChecked = true;
        }

        public ProductModal(Product product) : this()
        {
            existingProduct = product;
            Title = "Update Product";

            if (existingProduct != null)
            {
                txtProductCode.Text = existingProduct.ProductCode;
                txtName.Text = existingProduct.Name;
                txtDescription.Text = existingProduct.Description;
                txtReorderLevel.Text = existingProduct.ReorderLevel.ToString();
                chkIsActive.IsChecked = existingProduct.IsActive;
                txtCreatedAt.Text = existingProduct.CreatedAt.ToString("yyyy-MM-dd HH:mm");

                if (existingProduct.SupplierId.HasValue)
                {
                    cmbSuppliers.SelectedValue = existingProduct.SupplierId.Value;
                }
                else
                {
                    cmbSuppliers.SelectedValue = 0;
                }
            }
        }

        private void LoadSuppliers()
        {
            suppliers = new List<Supplier>
            {
                new Supplier{ Id = 0, Name = "No supplier"}
            };

            suppliers.AddRange(supplierService.GetSuppliers());

            cmbSuppliers.DisplayMemberPath = "Name";
            cmbSuppliers.SelectedValuePath = "Id";
            cmbSuppliers.ItemsSource = suppliers;
            cmbSuppliers.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var productCode = txtProductCode.Text?.Trim();
            var name = txtName.Text?.Trim();

            if (!int.TryParse(txtReorderLevel.Text?.Trim(), out int reorderLevel) || reorderLevel < 0)
            {
                MessageBox.Show("Reorder level must be positive number");
                return;
            }

            int? supplierId = null;
            var selectedSupplier = cmbSuppliers.SelectedItem as Supplier;
            if (selectedSupplier != null && selectedSupplier.Id != 0)
            {
                supplierId = selectedSupplier.Id;
            }

            var product = new Product
            {
                Id = existingProduct?.Id ?? 0,
                ProductCode = productCode,
                Name = name,
                Description = txtDescription.Text?.Trim(),
                SupplierId = supplierId,
                ReorderLevel = reorderLevel,
                IsActive = chkIsActive.IsChecked == true,
                CreatedAt = existingProduct?.CreatedAt ?? DateTime.Now,
            };

            var validation = productService.ValidateProduct(product);
            if (!validation.IsSuccessful)
            {
                MessageBox.Show(validation.ErrorMessage);
                return;
            }

            bool isSuccessful;

            if (existingProduct != null)
            {
                isSuccessful = productService.UpdateProduct(product);
            }
            else
            {
                isSuccessful = productService.AddProduct(product);
            }

            if (!isSuccessful)
            {
                MessageBox.Show("Fatal error while saving product");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnHelp(object sender, ExecutedRoutedEventArgs e)
        {
            HelpService.ShowHelpForContext(this);
        }
    }
}
