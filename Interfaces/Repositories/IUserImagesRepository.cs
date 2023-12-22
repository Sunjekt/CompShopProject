using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IUserImagesRepository
    {
        List<UserImage> GetList();
        UserImage GetItem(int id);
        UserImage GetItemByUserId(int userId);

        void Create(UserImage item);
        void Update(UserImage item);
        void Delete(int id);
    }
}
