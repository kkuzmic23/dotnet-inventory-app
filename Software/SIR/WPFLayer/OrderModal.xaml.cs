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
using System.Windows.Shapes;

namespace WPFLayer
{
    /// <summary>
    /// Interaction logic for OrderModal.xaml
    /// </summary>
    public partial class OrderModal : Window
    {
        private readonly OrderService orderService = new OrderService();
        private readonly ProductService productservice = new ProductService();
        private readonly OrderItemService orderItemService = new OrderItemService();
        private readonly SupplierService supplierService = new SupplierService();


        private readonly ObservableCollection<OrderItem> items = new ObservableCollection<OrderItem>();
        private readonly Order existingOrder;
        private List<Product> products = new List<Product>();
        public OrderModal()
        {
            InitializeComponent();
            dgItems.ItemsSource = items;
            LoadSuppliers();
            LoadProducts();

            txtStatus.Text = "Created";
            dpCreatedAt.SelectedDate = DateTime.Now;
            items.Add(new OrderItem());
        }

        public OrderModal(Order order) : this()
        {
            existingOrder = order;
            Title = "Update order";

            if (existingOrder != null)
            {
                cmbSupplier.SelectedValue = existingOrder.SupplierId;
                txtStatus.Text = existingOrder.Status;
                dpCreatedAt.SelectedDate = existingOrder.CreatedAt;
                dpReceivedAt.SelectedDate = existingOrder.ReceivedAt;
                LoadItemsForOrder(existingOrder.Id);
            }
        }

        private void LoadSuppliers()
        {
            var suppliers = supplierService.GetSuppliers();
            cmbSupplier.DisplayMemberPath = "Name";
            cmbSupplier.SelectedValuePath = "Id";
            cmbSupplier.ItemsSource = suppliers;
        }

        private void LoadProducts()
        {
            products = productservice.GetProducts();
            colProduct.ItemsSource = products;
        }

        private void LoadItemsForOrder(int orderId)
        {
            items.Clear();
            var existingItems = orderItemService.GetItemsByOrderId(orderId);
            foreach (var item in existingItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId) ?? item.Product;
                items.Add(new OrderItem
                {
                    Product = product,
                    Quantity = item.Quantity
                });
            }

            if (items.Count == 0)
            {
                items.Add(new OrderItem());
            }
        }
    }
}
