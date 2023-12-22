using DAL.Manager;
using Interfaces.Repositories;
using DomainModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private ModelsManager db;

        public UsersRepository(ModelsManager dbcontext)
        {
            this.db = dbcontext;
        }

        public List<User> GetList()
        {
            return db.User.ToList();
        }

        public List<User> GetListOfCustomers()
        {
            return db.User.Where(user => user.RoleId == 2).ToList();
        }

        public List<User> GetListByContaintsLetters(string letters)
        {
            return db.User.Where(user => user.Name.Contains(letters) && user.RoleId == 2).ToList();
        }

        public User GetItem(int id)
        {
            return db.User.Find(id);
        }

        public User GetItemByName(string userName)
        {
            return db.User.Where(user => user.Name == userName).FirstOrDefault();
        }

        public void Create(User item)
        {
            db.User.Add(item);
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User item = db.User.Find(id);
            if (item != null)
                db.User.Remove(item);
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }

    }
}
