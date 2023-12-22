using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IProducersRepository
    {
        List<Producer> GetList();
        List<Producer> GetListByContaintsLetters(string phrase);
        Producer GetItem(int id);
        Producer GetItemByName(string name);

        void Create(Producer item);
        void Update(Producer item);
        void Delete(int id);
    }
}
