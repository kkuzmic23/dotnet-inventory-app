using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class OrderItemService
    {
        IOrderItemCRUDRepository repo;

        public OrderItemService() : this(new OrderHasProductRepository())
        {
        }

        public OrderItemService(IOrderItemCRUDRepository orderItemRepository)
        {
            repo = orderItemRepository;
        }

        public List<OrderHasProduct> GetItemsByOrderId(int orderId)
        {
            return repo.GetAll().Where(x => x.OrderId == orderId).ToList();
        }

        public bool ReplaceItems(int orderId, IEnumerable<OrderHasProduct> items)
        {
            if (!ValidateOrderItems(items).IsSuccessful)
            {
                return false;
            }

            repo.RemoveByOrderId(orderId);

            foreach(var item in items)
            {
                repo.Add(item);
            }
            repo.SaveChanges();
            return true;
        }

        public ServiceResult ValidateOrderItems(IEnumerable<OrderHasProduct> items)
        {
            if (items == null || !items.Any())
            {
                return ServiceResult.Failure("Add at least one item");
            }

            if (items.Any(x => x.ProductId <= 0))
            {
                return ServiceResult.Failure("An order item cannot be empty");
            }

            if (items.Any(x => x.Quantity <= 0))
            {
                return ServiceResult.Failure("An order item cannot have 0 or less");
            }

            if (items.GroupBy(x => x.ProductId).Any(g => g.Count() > 1))
            {
                return ServiceResult.Failure("Cannot have the same product more than once");
            }

            return ServiceResult.Success();
        }
    }
}
