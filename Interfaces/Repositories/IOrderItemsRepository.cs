using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IOrderItemsRepository
    {
        List<OrderItem> GetList();
        List<OrderItem> GetListByOrderId(int orderId);
        OrderItem GetItem(int id);

        void Create(OrderItem item);
        void Update(OrderItem item);
        void Delete(int id);
    }
}
