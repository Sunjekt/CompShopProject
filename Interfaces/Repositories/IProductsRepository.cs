using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IProductsRepository
    {
        List<Product> GetList();
        List<Product> GetListByCategoryId(int categoryId);
        List<Product> GetListByProducerId(int producerId);
        List<Product> GetExistingList();
        List<Product> GetExistingListByCategoryId(int categoryId);
        List<Product> GetListByProducerAndCategoryId(int producerId, int categoryId);
        List<Product> GetExistingListByProducerAndCategoryId(int producerId, int categoryId);
        List<Product> GetExistingListByProducerId(int producerId);
        Product GetItem(int id);
        Product GetItemByName(string productName);

        void Create(Product item);
        void Update(Product item);
        void Delete(int id);
    }
}
