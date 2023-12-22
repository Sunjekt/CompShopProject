using Interfaces.Repositories;
using DomainModel.Models;
using System.Collections.Generic;
using Interfaces.Services;

namespace BLL.Services
{

    public class OrdersService : IOrdersService
    {
        private IDbRepos db;
        public OrdersService(IDbRepos repos)
        {
            db = repos;
        }

        public void AddOrder(Order order, int userId)
        {
            db.Orders.Create(order);
            var cartItems = db.CartItems.GetListByUsertId(userId);

            foreach (var cartItem in cartItems)
            {
                OrderItem orderItem = new OrderItem()
                {
                    ProductId = cartItem.ProductId,
                    Price = cartItem.Product.Price * cartItem.Quantity,
                    Quantity = cartItem.Quantity,
                    OrderId = order.Id
                };

                cartItem.Product.Quantity -= cartItem.Quantity;

                db.OrderItems.Create(orderItem);
                db.CartItems.Delete(cartItem.Id);
            }
            db.Save();
        }

        public void CancelOrder(Order canceledOrder)
        {
            var order = db.Orders.GetItem(canceledOrder.Id);
            if (order != null)
            {
                order.User.Id = canceledOrder.UserId;
                order.CreationDate = canceledOrder.CreationDate;
                order.StatusId = canceledOrder.StatusId;
            }

            var orderItems = db.OrderItems.GetListByOrderId(order.Id);
            foreach (var orderItem in orderItems)
            {
                orderItem.Product.Quantity += orderItem.Quantity;
                db.OrderItems.Update(orderItem);
            }
            db.Orders.Update(order);
            db.Save();
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return db.Orders.GetListByUserId(userId);
        }

        public List<Order> GetAllOrders()
        {
            return db.Orders.GetList();
        }

        public List<Order> GetOrdersByMonthAndYear(int month, int year)
        {
            return db.Orders.GetListByMonthAndYear(month, year);
        }

        public List<Order> GetOrdersByMonth(int month)
        {
            return db.Orders.GetListByMonth(month);
        }

        public List<Order> GetOrdersByYear(int year)
        {
            return db.Orders.GetListByYear(year);
        }

        public List<Order> GetOrdersByMonthAndYearAndStatus(int status, int month, int year)
        {
            return db.Orders.GetListByMonthAndYearAndStatus(status, month, year);
        }

        public List<Order> GetOrdersByMonthAndStatus(int status, int month)
        {
            return db.Orders.GetListByMonthAndStatus(status, month);
        }

        public List<Order> GetOrdersByStatus(int status)
        {
            return db.Orders.GetListByStatus(status);
        }

        public List<Order> GetOrdersByYearAndStatus(int status, int year)
        {
            return db.Orders.GetListByYearAndStatus(status, year);
        }

        public void UpdateOrder(Order changedOrder)
        {
            var order = db.Orders.GetItem(changedOrder.Id);
            if (order != null)
            {
                order.User.Id = changedOrder.UserId;
                order.CreationDate = changedOrder.CreationDate;
                order.StatusId = changedOrder.StatusId;

                db.Orders.Update(order);
                db.Save();
            }
        }
    }

}
