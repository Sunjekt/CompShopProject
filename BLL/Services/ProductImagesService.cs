using Interfaces.Repositories;
using DomainModel.Models;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Interfaces.Services;

namespace BLL.Services
{
    public class ProductImagesService : IProductImagesService
    {
        private IDbRepos db;
        public ProductImagesService(IDbRepos repos)
        {
            db = repos;
        }

        public void AddImages(string[] pathes, int productId)
        {
            if (pathes.Length > 0)
            {
                byte[] buff;
                for (int i = 0; i < pathes.Length; i++)
                {
                    if (File.Exists(pathes[i]))
                    {
                        buff = File.ReadAllBytes(pathes[i]);
                        Image img = Image.FromFile(pathes[i]);
                        Bitmap resizedImage = new Bitmap(img, new System.Drawing.Size(256, 256));
                        using (var stream = new MemoryStream())
                        {
                            resizedImage.Save(stream, ImageFormat.Jpeg);
                            byte[] bytes = stream.ToArray();

                            db.ProductImages.Create(new ProductImage()
                            {
                                FileExtension = Path.GetExtension(pathes[i]),
                                Image = bytes,
                                Size = bytes.Length,
                                ProductId = productId
                            });
                        }
                    }
                }
                db.Save();
            }
        }

        public void DeleteProductImagesByProductId(int productId)
        {
            var productImages = db.ProductImages.GetListByProductId(productId);
            foreach (var img in productImages)
                db.ProductImages.Delete(img.Id);
            db.Save();
        }

        public IEnumerable<ProductImage> GetImagesByProductId(int productId)
        {
            return db.ProductImages.GetListByProductId(productId);
        }
    }
}
