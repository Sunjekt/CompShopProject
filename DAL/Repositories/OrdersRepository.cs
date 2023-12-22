using DAL.Manager;
using Interfaces.Repositories;
using DomainModel.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private ModelsManager db;

        public OrdersRepository(ModelsManager dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Order> GetList()
        {
            return db.Order.ToList();
        }

        public List<Order> GetListByUserId(int userId)
        {
            return db.Order.Where(b => b.UserId == userId).ToList();
        }

        public List<Order> GetListByMonthAndYear(int month, int year)
        {
            return db.Order.Where(b => b.CreationDate.Month == month && b.CreationDate.Year == year).ToList();
        }

        public List<Order> GetListByMonth(int month)
        {
            return db.Order.Where(b => b.CreationDate.Month == month).ToList();
        }

        public List<Order> GetListByYear(int year)
        {
            return db.Order.Where(b => b.CreationDate.Year == year).ToList();
        }

        public List<Order> GetListByMonthAndYearAndStatus(int status, int month, int year)
        {
            return db.Order.Where(b => b.CreationDate.Month == month && b.CreationDate.Year == year && b.StatusId == status).ToList();
        }

        public List<Order> GetListByMonthAndStatus(int status, int month)
        {
            return db.Order.Where(b => b.CreationDate.Month == month && b.StatusId == status).ToList();
        }

        public List<Order> GetListByStatus(int status)
        {
            return db.Order.Where(b => b.StatusId == status).ToList();
        }

        public List<Order> GetListByYearAndStatus(int status, int year)
        {
            return db.Order.Where(b => b.CreationDate.Year == year && b.StatusId == status).ToList();
        }

        public Order GetItem(int id)
        {
            return db.Order.Find(id);
        }

        public void Create(Order item)
        {
            db.Order.Add(item);
        }

        public void Update(Order item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Order item = db.Order.Find(id);
            if (item != null)
                db.Order.Remove(item);
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }
    }
}
