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
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, OnHelp));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Help, new KeyGesture(Key.F1)));
            dgItems.ItemsSource = items;
            LoadSuppliers();
            LoadProducts();

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

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            items.Add(new OrderItem());
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgItems.SelectedItem as OrderItem;
            if (selected != null)
            {
                items.Remove(selected);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedSupplier = cmbSupplier.SelectedItem as Supplier;
            if (selectedSupplier == null)
            {
                MessageBox.Show("Select a supplier first");
                return;
            }

            var status = string.IsNullOrWhiteSpace(existingOrder?.Status) ? "Pending" : existingOrder.Status;

            if (!dpCreatedAt.SelectedDate.HasValue)
            {
                MessageBox.Show("Created date is required");
                return;
            }

            if (items.Count == 0)
            {
                MessageBox.Show("Add at least one item");
                return;
            }

            var invalidProduct = items.FirstOrDefault(x => x.Product == null);
            if (invalidProduct != null)
            {
                MessageBox.Show("An order item cannot be empty");
                return;
            }

            var invalidQuantity = items.FirstOrDefault(x => x.Quantity <= 0);
            if (invalidQuantity != null)
            {
                MessageBox.Show("An order item cannot have 0 or less");
                return;
            }

            var duplicateProduct = items.GroupBy(x => x.Product.Id).FirstOrDefault(g => g.Count() > 1);

            if (duplicateProduct != null)
            {
                MessageBox.Show("Cannot have the same product more than once");
                return;
            }



            var order = new Order
            {
                Id = existingOrder?.Id ?? 0,
                SupplierId = selectedSupplier.Id,
                Status = status,
                CreatedAt = dpCreatedAt.SelectedDate.Value,
                ReceivedAt = dpReceivedAt.SelectedDate
            };

            bool isSuccessful;

            if (existingOrder != null)
            {
                isSuccessful = orderService.UpdateOrder(order);
            }
            else
            {
                isSuccessful = orderService.AddOrder(order);
            }

            if (!isSuccessful || order.Id <= 0)
            {
                MessageBox.Show("Fatal flaw while saving order");
                return;
            }

            var orderItems = items.Select(x => new OrderHasProduct
            {
                OrderId = order.Id,
                ProductId = x.Product.Id,
                Quantity = x.Quantity
            }).ToList();

            bool itemsSaved = orderItemService.ReplaceItems(order.Id, orderItems);
            if (!itemsSaved)
            {
                MessageBox.Show("Order saved, but items save failed");
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
