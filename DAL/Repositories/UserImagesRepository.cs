using DAL.Manager;
using Interfaces.Repositories;
using DomainModel.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace DAL.Repositories
{

    public class UserImagesRepository : IUserImagesRepository
    {
        private ModelsManager db;

        public UserImagesRepository(ModelsManager dbcontext)
        {
            this.db = dbcontext;
        }

        public List<UserImage> GetList()
        {
            return db.UserImage.ToList();
        }

        public UserImage GetItem(int id)
        {
            return db.UserImage.Find(id);
        }

        public UserImage GetItemByUserId(int userId)
        {
            return db.UserImage.Where(i => i.UserId == userId).First();
        }

        public void Create(UserImage item)
        {
            db.UserImage.Add(item);
        }

        public void Update(UserImage item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            UserImage item = db.UserImage.Find(id);
            if (item != null)
                db.UserImage.Remove(item);
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }

    }
}
