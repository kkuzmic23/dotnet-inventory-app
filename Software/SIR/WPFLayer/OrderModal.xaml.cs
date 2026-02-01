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
    /// Interaction logic for OrderModal.xaml
    /// </summary>
    public partial class OrderModal : Window
    {
        private readonly OrderService orderService = new OrderService();
        private readonly ProductService productservice = new ProductService();



        //private readonly IObservableCollection<OrderItem> items = new IObservableCollection<OrderItem>();
        public OrderModal()
        {
            InitializeComponent();
        }

        public OrderModal(Order order) : this()
        {

        }
    }
}
