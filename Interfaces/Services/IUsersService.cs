using DomainModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IUsersService
    {
        void AddUser(User newUser);
        void UpdateUser(User changedUser);
        User FindUserByName(string userName);
        void DeleteUser(int userId);
        List<User> GetAllUsers();
        List<User> GetUsersByContaintsLetters(string letters);
    }
}
