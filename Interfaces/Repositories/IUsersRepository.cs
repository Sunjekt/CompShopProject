using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IUsersRepository
    {
        List<User> GetList();
        List<User> GetListOfCustomers();
        List<User> GetListByContaintsLetters(string letters);
        User GetItem(int id);
        User GetItemByName(string userName);

        void Create(User item);
        void Update(User item);
        void Delete(int id);
    }
}
