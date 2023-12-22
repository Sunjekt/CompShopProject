using DomainModel.Models;

namespace Interfaces.Services
{
    public interface IUserImagesService
    {
        void AddImageByUserId(string path, int userId);
        void DeleteImageByUserId(int userId);
        UserImage GetImageByUserId(int userId);
        void UpdateImageByUserId(int userId, UserImage image);
    }
}
