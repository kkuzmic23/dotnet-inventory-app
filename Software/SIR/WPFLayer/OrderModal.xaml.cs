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
            var status = string.IsNullOrWhiteSpace(existingOrder?.Status) ? "Pending" : existingOrder.Status;

            if (!dpCreatedAt.SelectedDate.HasValue)
            {
                MessageBox.Show("Created date is required");
                return;
            }

            var order = new Order
            {
                Id = existingOrder?.Id ?? 0,
                SupplierId = selectedSupplier?.Id ?? 0,
                Status = status,
                CreatedAt = dpCreatedAt.SelectedDate.Value,
                ReceivedAt = dpReceivedAt.SelectedDate
            };

            var orderValidation = orderService.ValidateOrder(order);
            if (!orderValidation.IsSuccessful)
            {
                MessageBox.Show(orderValidation.ErrorMessage);
                return;
            }

            var orderItems = items.Select(x => new OrderHasProduct
            {
                OrderId = order.Id,
                ProductId = x.Product?.Id ?? 0,
                Quantity = x.Quantity
            }).ToList();

            var itemValidation = orderItemService.ValidateOrderItems(orderItems);
            if (!itemValidation.IsSuccessful)
            {
                MessageBox.Show(itemValidation.ErrorMessage);
                return;
            }

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

            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = order.Id;
            }

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
