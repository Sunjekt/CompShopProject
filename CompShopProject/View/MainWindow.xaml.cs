using System.Windows;
using System.Windows.Input;
using Interfaces.Services;
using DomainModel.Models;
using CompShopProject.ViewModel;

namespace CompShopProject.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICartItemsService cartItemsService;
        private readonly ICategoriesService categoriesService;
        private readonly IOrderItemsService orderItemsService;
        private readonly IOrdersService ordersService;
        private readonly IProducersService producersService;
        private readonly IProductImagesService productImagesService;
        private readonly IProductsService productsService;
        private readonly IStatusService statusService;
        private readonly IUserImagesService userImagesService;
        private readonly IUsersService usersService;
        public MainWindow(User user, ICartItemsService cartItemsService, ICategoriesService categoriesService, IOrderItemsService orderItemsService, IOrdersService ordersService, IProducersService producersService, IProductImagesService productImagesService, IProductsService productsService, IStatusService statusService, IUserImagesService userImagesService, IUsersService usersService)
        {
            InitializeComponent();

            this.cartItemsService = cartItemsService;
            this.categoriesService = categoriesService;
            this.orderItemsService = orderItemsService;
            this.ordersService = ordersService;
            this.producersService = producersService;
            this.productImagesService = productImagesService;
            this.productsService = productsService;
            this.statusService = statusService;
            this.userImagesService = userImagesService;
            this.usersService = usersService;

            this.DataContext = new MainViewModel(user, cartItemsService, categoriesService, orderItemsService, ordersService, producersService, productImagesService, productsService, statusService, userImagesService, usersService);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
