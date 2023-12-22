using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface ICategoriesRepository
    {
        List<Category> GetList();
        List<Category> GetListByContaintsLetters(string phrase);
        Category GetItem(int id);
        Category GetItemByName(string name);

        void Create(Category item);
        void Update(Category item);
        void Delete(int id);
    }
}
