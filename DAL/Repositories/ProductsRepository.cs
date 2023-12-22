using DomainModel.Models;
using DAL.Manager;
using Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private ModelsManager db;

        public ProductsRepository(ModelsManager dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Product> GetList()
        {
            return db.Product.ToList();
        }

        public List<Product> GetListByCategoryId(int categoryId)
        {
            return db.Product.Where(p => p.CategoryId == categoryId).ToList();
        }

        public List<Product> GetListByProducerId(int producerId)
        {
            return db.Product.Where(p => p.CategoryId == producerId).ToList();
        }

        public List<Product> GetExistingList()
        {
            return db.Product.Where(p => p.Deleted_at == null).ToList();
        }

        public List<Product> GetExistingListByCategoryId(int categoryId)
        {
            return db.Product.Where(p => p.CategoryId == categoryId && p.Deleted_at == null)?.ToList();
        }

        public List<Product> GetListByProducerAndCategoryId(int producerId, int categoryId)
        {
            return db.Product.Where(p => p.ProducerId == producerId && p.CategoryId == categoryId).ToList();
        }

        public List<Product> GetExistingListByProducerAndCategoryId(int producerId, int categoryId)
        {
            return db.Product.Where(p => p.ProducerId == producerId && p.CategoryId == categoryId && p.Deleted_at == null).ToList();
        }

        public List<Product> GetExistingListByProducerId(int producerId)
        {
            return db.Product.Where(p => p.ProducerId == producerId && p.Deleted_at == null).ToList();
        }

        public Product GetItem(int id)
        {
            return db.Product.Find(id);
        }

        public Product GetItemByName(string productName)
        {
            return db.Product.Where(p => p.Name.Equals(productName)).FirstOrDefault();
        }

        public void Create(Product item)
        {
            db.Product.Add(item);
        }

        public void Update(Product item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Product item = db.Product.Find(id);
            if (item != null)
                db.Product.Remove(item);
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }

    }
}
