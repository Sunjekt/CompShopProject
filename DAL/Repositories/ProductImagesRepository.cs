using DAL.Manager;
using Interfaces.Repositories;
using DomainModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class ProductImagesRepository : IProductImagesRepository
    {
        private ModelsManager db;

        public ProductImagesRepository(ModelsManager dbcontext)
        {
            this.db = dbcontext;
        }

        public List<ProductImage> GetList()
        {
            return db.ProductImage.ToList();
        }

        public ProductImage GetItem(int id)
        {
            return db.ProductImage.Find(id);
        }

        public void Create(ProductImage item)
        {
            db.ProductImage.Add(item);
        }

        public void Update(ProductImage item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ProductImage item = db.ProductImage.Find(id);
            if (item != null)
                db.ProductImage.Remove(item);
        }

        public List<ProductImage> GetListByProductId(int productId)
        {
            return db.ProductImage.Where(img => img.ProductId == productId).ToList();
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }

    }
}
