using Interfaces.Repositories;
using DomainModel.Models;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using Interfaces.Services;

namespace BLL.Services
{

    public class UserImagesService : IUserImagesService
    {
        private IDbRepos db;
        public UserImagesService(IDbRepos repos)
        {
            db = repos;
        }
        public void AddImageByUserId(string path, int userId)
        {
            if (path.Length > 0)
            {
                byte[] buff;
                if (File.Exists(path))
                {
                    buff = File.ReadAllBytes(path);
                    Image img = Image.FromFile(path);
                    Bitmap resizedImage = new Bitmap(img, new System.Drawing.Size(256, 256));
                    using (var stream = new MemoryStream())
                    {
                        resizedImage.Save(stream, ImageFormat.Jpeg);
                        byte[] bytes = stream.ToArray();

                        UserImage userImage = new UserImage();
                        userImage.FileExtension = Path.GetExtension(path);
                        userImage.Image = bytes;
                        userImage.Size = bytes.Length;
                        userImage.UserId = userId;
                        db.UserImages.Create(userImage);
                    }
                }
                db.Save();
            }
        }
        public void DeleteImageByUserId(int userId)
        {
            var img = db.UserImages.GetItemByUserId(userId);
            db.UserImages.Delete(img.Id);
            db.Save();
        }

        public UserImage GetImageByUserId(int userId)
        {
            return db.UserImages.GetItemByUserId(userId);
        }

        public void UpdateImageByUserId(int userId, UserImage image)
        {
            var findImage = db.UserImages.GetItemByUserId(userId);
            if (findImage != null)
            {
                db.UserImages.Delete(findImage.Id);
                db.UserImages.Create(image);
                db.Save();
            }
        }
    }

}
