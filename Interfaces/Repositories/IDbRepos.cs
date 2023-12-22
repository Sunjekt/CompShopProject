using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IDbRepos
    {
        ICartItemsRepository CartItems { get; }
        ICategoriesRepository Categories { get; }
        IOrderItemsRepository OrderItems { get; }
        IOrdersRepository Orders { get; }
        IProducersRepository Producers { get; }
        IProductImagesRepository ProductImages { get; }
        IProductsRepository Products { get; }
        IStatusRepository Statuses { get; }
        IUserImagesRepository UserImages { get; }
        IUsersRepository Users { get; }
        int Save();
    }
}
