using DomainModel.Models;
using System.Collections.Generic;

namespace Interfaces.Services
{
    public interface IProductsService
    {
        List<Product> GetAllProducts();
        List<Product> GetAllExistingProducts();
        List<Product> GetProductsByProducerId(int producerId);
        List<Product> GetExistingProductsByProducerId(int producerId);
        List<Product> GetProductsByCategoryId(int categoryId);
        List<Product> GetExistingProductsByCategoryId(int categoryId);
        List<Product> GetProductsByProducerAndCategoryId(int producerId, int categoryId);
        List<Product> GetExistingProductsByProducerAndCategoryId(int producerId, int categoryId);
        Product GetProductById(int productId);
        Product GetProductByName(string productName);
        void AddNewProduct(Product product);
        void UpdateProduct(Product changedProduct);
        void DeleteProductById(int productId);
        void DeleteProductsByCategoryId(int categoryId);
        void DeleteProductsByProducerId(int producerId);
    }
}
