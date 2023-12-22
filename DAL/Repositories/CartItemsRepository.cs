using DomainModel.Models;
using DAL.Manager;
using Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DAL.Repositories
{

    public class CartItemsRepository : ICartItemsRepository
    {
        private ModelsManager db;

        public CartItemsRepository(ModelsManager dbcontext)
        {
            this.db = dbcontext;
        }

        public List<CartItem> GetList()
        {
            return db.CartItem.ToList();
        }

        public List<CartItem> GetListByProductId(int productId)
        {
            return db.CartItem.Where(p => p.ProductId == productId).ToList();
        }

        public List<CartItem> GetListByUsertId(int userId)
        {
            return db.CartItem.Where(p => p.UserId == userId).ToList();
        }

        public CartItem GetItemByUserIdAndProductId(int userId, int productId)
        {
            return db.CartItem.Where(c => c.UserId == userId && c.ProductId == productId).SingleOrDefault();
        }

        public CartItem GetItem(int id)
        {
            return db.CartItem.Find(id);
        }

        public void Create(CartItem item)
        {
            db.CartItem.Add(item);
        }

        public void Update(CartItem item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            CartItem item = db.CartItem.Find(id);
            if (item != null)
                db.CartItem.Remove(item);
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }

    }
}
