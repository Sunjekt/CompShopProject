using DomainModel.Models;
using Interfaces.Repositories;
using System.Collections.Generic;
using System;
using Interfaces.Services;

namespace BLL.Services
{
    public class ProductsService : IProductsService
    {
        private IDbRepos db;
        public ProductsService(IDbRepos repos)
        {
            db = repos;
        }
        public void AddNewProduct(Product product)
        {
            db.Products.Create(product);
            db.Save();
        }

        public void DeleteProductById(int productId)
        {
            var product = GetProductById(productId);
            product.Deleted_at = DateTime.Now;
            db.Products.Update(product);
            db.Save();
        }

        public void DeleteProductsByCategoryId(int categoryId)
        {
            var products = db.Products.GetListByCategoryId(categoryId);
            foreach (var product in products)
            {
                product.Deleted_at = DateTime.Now;
                db.Products.Update(product);
            }
            db.Save();
        }

        public void DeleteProductsByProducerId(int producerId)
        {
            var products = db.Products.GetListByProducerId(producerId);
            foreach (var product in products)
            {
                product.Deleted_at = DateTime.Now;
                db.Products.Update(product);
            }
            db.Save();
        }

        public List<Product> GetAllProducts()
        {
            return db.Products.GetList();
        }

        public List<Product> GetAllExistingProducts()
        {
            return db.Products.GetExistingList();
        }

        public Product GetProductById(int productId)
        {
            return db.Products.GetItem(productId);
        }

        public Product GetProductByName(string productName)
        {
            return db.Products.GetItemByName(productName);
        }
        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return db.Products.GetListByCategoryId(categoryId);
        }

        public List<Product> GetExistingProductsByCategoryId(int categoryId)
        {
            return db.Products.GetExistingListByCategoryId(categoryId);
        }

        public List<Product> GetProductsByProducerAndCategoryId(int producerId, int categoryId)
        {
            return db.Products.GetListByProducerAndCategoryId(producerId, categoryId);
        }

        public List<Product> GetExistingProductsByProducerAndCategoryId(int producerId, int categoryId)
        {
            return db.Products.GetExistingListByProducerAndCategoryId(producerId, categoryId);
        }

        public List<Product> GetProductsByProducerId(int producerId)
        {
            return db.Products.GetListByProducerId(producerId);
        }

        public List<Product> GetExistingProductsByProducerId(int producerId)
        {
            return db.Products.GetExistingListByProducerId(producerId);
        }

        public void UpdateProduct(Product changedProduct)
        {
            var product = db.Products.GetItem(changedProduct.Id);
            if (product.Deleted_at == null)
            {
                product.Name = changedProduct.Name;
                product.Price = changedProduct.Price;
                product.Description = changedProduct.Description;
                product.Rate = changedProduct.Rate;
                product.ImageBytes = changedProduct.ImageBytes;
                product.CreationDate = changedProduct.CreationDate;
                product.Quantity = changedProduct.Quantity;
                product.ProducerId = changedProduct.ProducerId;
                product.CategoryId = changedProduct.CategoryId;
            }
            else
            {
                product.Deleted_at = changedProduct.Deleted_at;
            }
            db.Products.Update(product);
            db.Save();
        }
    }
}
