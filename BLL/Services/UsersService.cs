using Interfaces.Repositories;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using Interfaces.Services;

namespace BLL.Services
{
    public class UsersService : IUsersService
    {
        private IDbRepos db;
        public UsersService(IDbRepos repos)
        {
            db = repos;
        }
        public void AddUser(User newUser)
        {
            db.Users.Create(newUser);
            db.Save();
        }

        public void DeleteUser(int userId)
        {
            var user = db.Users.GetItem(userId);
            user.Deleted_at = DateTime.Now;
            db.Users.Update(user);
            db.Save();
        }

        public User FindUserByName(string userName)
        {
            return db.Users.GetItemByName(userName);
        }

        public List<User> GetAllUsers()
        {
            return db.Users.GetListOfCustomers();
        }

        public List<User> GetUsersByContaintsLetters(string letters)
        {
            return db.Users.GetListByContaintsLetters(letters);
        }

        public void UpdateUser(User changedUser)
        {
            var user = db.Users.GetItem(changedUser.Id);
            if (user != null)
            {
                db.Users.Update(changedUser);
                db.Save();
            }
        }
    }
}
