using DomainModel.Models;
using System.Collections.Generic;

namespace Interfaces.Services
{
    public interface IOrderItemsService
    {
        void AddOrderItem(OrderItem order);
        List<OrderItem> GetOrderItemsByOrderId(int orderId);
    }
}
