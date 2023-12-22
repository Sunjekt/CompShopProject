using DomainModel.Models;
using System.Collections.Generic;

namespace Interfaces.Services
{
    public interface ICartItemsService
    {
        void AddCartItem(int userId, int productId);
        void UpdateCartItem(CartItem changedCartItem);
        CartItem GetCartItemById(int cartItemId);
        void DeleteCartItemById(int cartItemId);
        void DeleteCartItemsByProductId(int productId);
        List<CartItem> GetCartItemsByUserId(int userId);

    }
}
