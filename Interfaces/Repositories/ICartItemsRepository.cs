using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface ICartItemsRepository
    {
        List<CartItem> GetList();
        List<CartItem> GetListByProductId(int productId);
        List<CartItem> GetListByUsertId(int userId);
        CartItem GetItem(int id);
        CartItem GetItemByUserIdAndProductId(int userId, int productId);

        void Create(CartItem item);
        void Update(CartItem item);
        void Delete(int id);
    }
}
