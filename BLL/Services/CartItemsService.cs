using DomainModel.Models;
using Interfaces.Repositories;
using Interfaces.Services;
using System.Collections.Generic;

namespace BLL.Services
{

    public class CartItemsService : ICartItemsService
    {
        private IDbRepos db;
        public CartItemsService(IDbRepos repos)
        {
            db = repos;
        }

        public void AddCartItem(int userId, int productId)
        {
            CartItem cartItem = db.CartItems.GetItemByUserIdAndProductId(userId, productId);
            if (cartItem == null)
            {
                cartItem = new CartItem()
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = 1,
                };
                db.CartItems.Create(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            db.Save();

        }

        public void UpdateCartItem(CartItem changedCartItem)
        {
            var cartItem = db.CartItems.GetItem(changedCartItem.Id);
            cartItem.UserId = changedCartItem.UserId;
            cartItem.ProductId = changedCartItem.ProductId;
            cartItem.Quantity = changedCartItem.Quantity;
            db.CartItems.Update(cartItem);
            db.Save();
        }
        public CartItem GetCartItemById(int cartItemId)
        {
            return db.CartItems.GetItem(cartItemId);
        }
        public void DeleteCartItemById(int cartItemId)
        {
            db.CartItems.Delete(cartItemId);
            db.Save();
        }

        public void DeleteCartItemsByProductId(int productId)
        {
            var cartItems = db.CartItems.GetListByProductId(productId);
            foreach (var cartItem in cartItems)
            {
                db.CartItems.Delete(cartItem.Id);
            }
            db.Save();
        }

        public List<CartItem> GetCartItemsByUserId(int userId)
        {
            return db.CartItems.GetListByUsertId(userId);
        }
    }
}
