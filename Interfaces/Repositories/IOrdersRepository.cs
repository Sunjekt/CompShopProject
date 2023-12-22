using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IOrdersRepository
    {
        List<Order> GetList();
        List<Order> GetListByUserId(int userId);
        List<Order> GetListByMonthAndYear(int month, int year);
        List<Order> GetListByMonth(int month);
        List<Order> GetListByYear(int year);
        List<Order> GetListByMonthAndYearAndStatus(int status, int month, int year);
        List<Order> GetListByMonthAndStatus(int status, int month);
        List<Order> GetListByStatus(int status);
        List<Order> GetListByYearAndStatus(int status, int year);
        Order GetItem(int id);

        void Create(Order item);
        void Update(Order item);
        void Delete(int id);
    }
}
