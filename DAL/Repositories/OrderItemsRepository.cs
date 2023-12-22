using DomainModel.Models;
using DAL.Manager;
using Interfaces.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repositories
{

    public class OrderItemsRepository : IOrderItemsRepository
    {
        private ModelsManager db;

        public OrderItemsRepository(ModelsManager dbcontext)
        {
            this.db = dbcontext;
        }

        public List<OrderItem> GetList()
        {
            return db.OrderItem.ToList();
        }

        public List<OrderItem> GetListByOrderId(int orderId)
        {
            return db.OrderItem.Where(p => p.OrderId == orderId).ToList();
        }

        public OrderItem GetItem(int id)
        {
            return db.OrderItem.Find(id);
        }

        public void Create(OrderItem item)
        {
            db.OrderItem.Add(item);
        }

        public void Update(OrderItem item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            OrderItem item = db.OrderItem.Find(id);
            if (item != null)
                db.OrderItem.Remove(item);
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }

    }

}
