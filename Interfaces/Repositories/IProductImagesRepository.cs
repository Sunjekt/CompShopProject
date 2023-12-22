using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IProductImagesRepository
    {
        List<ProductImage> GetList();
        List<ProductImage> GetListByProductId(int productId);
        ProductImage GetItem(int id);

        void Create(ProductImage item);
        void Update(ProductImage item);
        void Delete(int id);
    }
}
