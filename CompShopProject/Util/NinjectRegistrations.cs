using BLL.Services;
using Interfaces.Services;
using Ninject.Modules;

namespace CompShopProject.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<ICartItemsService>().To<CartItemsService>();
            Bind<ICategoriesService>().To<CategoriesService>();
            Bind<IOrderItemsService>().To<OrderItemsService>();
            Bind<IOrdersService>().To<OrdersService>();
            Bind<IProducersService>().To<ProducersService>();
            Bind<IProductImagesService>().To<ProductImagesService>();
            Bind<IProductsService>().To<ProductsService>();
            Bind<IStatusService>().To<StatusService>();
            Bind<IUserImagesService>().To<UserImagesService>();
            Bind<IUsersService>().To<UsersService>();
        }
    }
}
