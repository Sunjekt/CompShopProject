using DomainModel.Models;
using System.Collections.Generic;

namespace Interfaces.Services
{
    public interface IProductImagesService
    {
        void AddImages(string[] pathes, int productId);
        IEnumerable<ProductImage> GetImagesByProductId(int productId);
        void DeleteProductImagesByProductId(int productId);
    }
}
