using DAL.Manager;
using Interfaces.Repositories;

namespace DAL.Repositories
{
    public class DbRepos : IDbRepos
    {
        private ModelsManager db;
        private CartItemsRepository cartItemsRepository;
        private CategoriesRepository categoriesRepository;
        private OrderItemsRepository orderItemsRepository;
        private OrdersRepository ordersRepository;
        private ProducersRepository producersRepository;
        private ProductImagesRepository productImagesRepository;
        private ProductsRepository productsRepository;
        private StatusRepository statusRepository;
        private UserImagesRepository userImagesRepository;
        private UsersRepository usersRepository;

        public DbRepos()
        {
            db = new ModelsManager();
        }

        public ICartItemsRepository CartItems
        {
            get
            {
                if (cartItemsRepository == null)
                    cartItemsRepository = new CartItemsRepository(db);
                return cartItemsRepository;
            }
        }

        public ICategoriesRepository Categories
        {
            get
            {
                if (categoriesRepository == null)
                    categoriesRepository = new CategoriesRepository(db);
                return categoriesRepository;
            }
        }

        public IOrderItemsRepository OrderItems
        {
            get
            {
                if (orderItemsRepository == null)
                    orderItemsRepository = new OrderItemsRepository(db);
                return orderItemsRepository;
            }
        }

        public IOrdersRepository Orders
        {
            get
            {
                if (ordersRepository == null)
                    ordersRepository = new OrdersRepository(db);
                return ordersRepository;
            }
        }

        public IProducersRepository Producers
        {
            get
            {
                if (producersRepository == null)
                    producersRepository = new ProducersRepository(db);
                return producersRepository;
            }
        }

        public IProductImagesRepository ProductImages
        {
            get
            {
                if (productImagesRepository == null)
                    productImagesRepository = new ProductImagesRepository(db);
                return productImagesRepository;
            }
        }

        public IProductsRepository Products
        {
            get
            {
                if (productsRepository == null)
                    productsRepository = new ProductsRepository(db);
                return productsRepository;
            }
        }

        public IStatusRepository Statuses
        {
            get
            {
                if (statusRepository == null)
                    statusRepository = new StatusRepository(db);
                return statusRepository;
            }
        }

        public IUserImagesRepository UserImages
        {
            get
            {
                if (userImagesRepository == null)
                    userImagesRepository = new UserImagesRepository(db);
                return userImagesRepository;
            }
        }

        public IUsersRepository Users
        {
            get
            {
                if (usersRepository == null)
                    usersRepository = new UsersRepository(db);
                return usersRepository;
            }
        }

        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
