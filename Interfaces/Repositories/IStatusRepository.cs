using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IStatusRepository
    {
        List<Status> GetList();
        Status GetItem(int id);

        void Create(Status item);
        void Update(Status item);
        void Delete(int id);
    }
}
