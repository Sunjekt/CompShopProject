using DomainModel.Models;
using Interfaces.Repositories;
using Interfaces.Services;
using System.Collections.Generic;

namespace BLL.Services
{

    public class OrderItemsService : IOrderItemsService
    {
        private IDbRepos db;
        public OrderItemsService(IDbRepos repos)
        {
            db = repos;
        }

        public void AddOrderItem(OrderItem order)
        {
            db.OrderItems.Create(order);
            db.Save();

        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return db.OrderItems.GetListByOrderId(orderId);
        }
    }

}
