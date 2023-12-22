using DomainModel.Models;
using System.Collections.Generic;

namespace Interfaces.Services
{
    public interface IOrdersService
    {
        void UpdateOrder(Order changedorder);
        void AddOrder(Order order, int userId);
        void CancelOrder(Order order);
        List<Order> GetOrdersByMonthAndYear(int month, int year);
        List<Order> GetOrdersByMonth(int month);
        List<Order> GetOrdersByYear(int year);
        List<Order> GetOrdersByMonthAndYearAndStatus(int status, int month, int year);
        List<Order> GetOrdersByMonthAndStatus(int status, int month);
        List<Order> GetOrdersByStatus(int status);
        List<Order> GetOrdersByYearAndStatus(int status, int year);
        List<Order> GetOrdersByUserId(int userId);
        List<Order> GetAllOrders();
    }
}
