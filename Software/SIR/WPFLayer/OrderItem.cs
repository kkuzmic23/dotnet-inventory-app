using EntityLayer.Entities;

namespace WPFLayer
{
    public class OrderItem
    {
        public OrderItem()
        {
            Quantity = 1;
        }

        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}