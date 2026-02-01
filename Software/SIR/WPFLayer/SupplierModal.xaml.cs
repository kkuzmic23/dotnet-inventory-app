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
    /// <summary>
    /// Interaction logic for SupplierModal.xaml
    /// </summary>
    public partial class SupplierModal : Window
    {
        private readonly SupplierService supplierService = new SupplierService();
        private readonly Supplier existingSupplier;
        public SupplierModal()
        {
            InitializeComponent();
        }

        public SupplierModal(Supplier supplier) : this()
        {
            existingSupplier = supplier;
            Title = "Update Supplier";

            if (existingSupplier != null)
            {
                txtName.Text = existingSupplier.Name;
                txtEmail.Text = existingSupplier.Email;
                txtPhone.Text = existingSupplier.Phone;
                txtAddress.Text = existingSupplier.Address;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var name = txtName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name is required");
                return;
            }

            var supplier = new Supplier
            {
                Id = existingSupplier?.Id ?? 0,
                Name = name,
                Email = txtEmail.Text?.Trim(),
                Phone = txtPhone.Text?.Trim(),
                Address = txtAddress.Text?.Trim(),
                CreatedAt = existingSupplier?.CreatedAt ?? DateTime.Now
            };

            bool isSuccessful;

            if (existingSupplier != null)
            {
                isSuccessful = supplierService.UpdateSupplier(supplier);
            }
            else
            {
                isSuccessful = supplierService.AddSupplier(supplier);
            }

            if (!isSuccessful)
            {
                MessageBox.Show("Fatal error while saving supplier");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
