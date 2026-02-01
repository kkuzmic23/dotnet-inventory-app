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
            cmbSupplier.SelectedValue = existingOrder.SupplierId;
            txtStatus.Text = existingOrder.Status;
            dpCreatedAt.SelectedDate = existingOrder.CreatedAt;
            dpReceivedAt.SelectedDate = existingOrder.ReceivedAt;
        }

        private void LoadSuppliers()
        {

        }

        private void LoadProducts()
        {

        }
    }
}
