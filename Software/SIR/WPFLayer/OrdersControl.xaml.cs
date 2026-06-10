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
    /// Interaction logic for OrdersControl.xaml
    /// </summary>
    public partial class OrdersControl : UserControl
    {
        private readonly OrderService orderService = new OrderService();
        private ObservableCollection<Order> orders = new ObservableCollection<Order>();
        public OrdersControl()
        {
            InitializeComponent();
            dgOrders.ItemsSource = orders;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            orders = new ObservableCollection<Order>(orderService.GetOrders());
            dgOrders.ItemsSource = orders;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrderModal
            {
                Owner = Window.GetWindow(this)
            };

            if (window.ShowDialog() == true)
            {
                LoadOrders();
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var order = GetSelectedOrder();
            if (order == null || order.Id == 0)
            {
                MessageBox.Show("Select an order first");
                return;
            }

            var window = new OrderModal(order)
            {
                Owner = Window.GetWindow(this)
            };

            if (window.ShowDialog() == true)
            {
                LoadOrders();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var order = GetSelectedOrder();
            if (order == null || order.Id == 0)
            {
                MessageBox.Show("Select an order first");
                return;
            }

            bool isSuccessful = orderService.RemoveOrder(order);
            if (!isSuccessful)
            {
                MessageBox.Show("Fatal error while removing selected order");
            }

            LoadOrders();
        }

        private Order GetSelectedOrder()
        {
            return dgOrders.SelectedItem as Order;
        }
    }
}
