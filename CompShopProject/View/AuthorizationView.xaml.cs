using CompShopProject.ViewModel;
using Interfaces.Services;
using System.Windows;
using System.Windows.Input;

namespace CompShopProject.View
{
    /// <summary>
    /// Interaction logic for AuthorizationView.xaml
    /// </summary>
    public partial class AuthorizationView : Window
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
        public AuthorizationView(ICartItemsService cartItemsService, ICategoriesService categoriesService, IOrderItemsService orderItemsService, IOrdersService ordersService, IProducersService producersService, IProductImagesService productImagesService, IProductsService productsService, IStatusService statusService, IUserImagesService userImagesService, IUsersService usersService)
        {
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

            this.DataContext = new AccountViewModel(cartItemsService, categoriesService, orderItemsService, ordersService, producersService, productImagesService, productsService, statusService, userImagesService, usersService);
            InitializeComponent();
        }


        private void crossButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
