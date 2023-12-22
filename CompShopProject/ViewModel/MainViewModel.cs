using CompShopProject.Core;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Interfaces.Services;
using DomainModel.Models;

namespace CompShopProject.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public RelayCommand ProductsViewCommand { get; set; }
        public RelayCommand AddProductViewCommand { get; set; }
        public RelayCommand CategoryViewCommand { get; set; }
        public RelayCommand ProducerViewCommand { get; set; }
        public RelayCommand ProfileViewCommand { get; set; }
        public RelayCommand UsersViewCommand { get; set; }
        public RelayCommand CartViewCommand { get; set; }
        public RelayCommand OrdersViewCommand { get; set; }

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

        #region Accessors
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        private Visibility _adminButtons;
        public Visibility AdminButtons
        {
            get { return _adminButtons; }
            set
            {
                _adminButtons = value;
                OnPropertyChanged(nameof(AdminButtons));
            }
        }
        private Visibility _userButtons;
        public Visibility UserButtons
        {
            get { return _userButtons; }
            set
            {
                _userButtons = value;
                OnPropertyChanged(nameof(UserButtons));
            }
        }
        private string _ordersButtonName;
        public string OrdersButtonName
        {
            get { return _ordersButtonName; }
            set
            {
                _ordersButtonName = value;
                OnPropertyChanged(nameof(OrdersButtonName));
            }
        }
        private object _imagePath;
        public object ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        #endregion
        public MainViewModel() { }
        public MainViewModel(User user, ICartItemsService cartItemsService, ICategoriesService categoriesService, IOrderItemsService orderItemsService, IOrdersService ordersService, IProducersService producersService, IProductImagesService productImagesService, IProductsService productsService, IStatusService statusService, IUserImagesService userImagesService, IUsersService usersService)
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

            CurrentUser = new User()
            {
                Id = user.Id,
                Password = user.Password,
                Role = user.Role,
                Name = user.Name,
                RoleId = user.RoleId,
                RegistrationDate = user.RegistrationDate,
                UserImage = user.UserImage
            };

            if (CurrentUser.Role.Name == "Администратор")
            {
                AdminButtons = Visibility.Visible;
                UserButtons = Visibility.Collapsed;
                OrdersButtonName = "Заказы";
            }
            else
            {
                AdminButtons = Visibility.Collapsed;
                UserButtons = Visibility.Visible;
                OrdersButtonName = "Мои покупки";
            }

            UserImage userImage = userImagesService.GetImageByUserId(user.Id);
            if (userImage != null)
                ImagePath = ConvertByteArrayToBitMapImage(userImage.Image);
            else
                ImagePath = @"../../Images/defUser.png";

            if (CurrentUser.Role.Name == "Администратор")
            {
                AddProductViewCommand = new RelayCommand(o => { CurrentView = new AddProductViewModel(productImagesService, categoriesService, producersService, productsService); });
                CategoryViewCommand = new RelayCommand(o => { CurrentView = new CategoryViewModel(categoriesService, productsService); });
                ProducerViewCommand = new RelayCommand(o => { CurrentView = new ProducerViewModel(producersService, productsService); });
                UsersViewCommand = new RelayCommand(o => { CurrentView = new UsersViewModel(usersService, userImagesService); });
            }
            else
            {
                CartViewCommand = new RelayCommand(o => { CurrentView = new CartViewModel(CurrentUser, productImagesService, cartItemsService, ordersService); });
            }

            CurrentView = new ProductsViewModel(CurrentUser, productsService, producersService, categoriesService, productImagesService, cartItemsService);

            ProductsViewCommand = new RelayCommand(o => { CurrentView = new ProductsViewModel(CurrentUser, productsService, producersService, categoriesService, productImagesService, cartItemsService); });
            ProfileViewCommand = new RelayCommand(o => { CurrentView = new ProfileViewModel(CurrentUser, ImagePath); });
            OrdersViewCommand = new RelayCommand(o => { CurrentView = new OrdersViewModel(CurrentUser, ordersService, orderItemsService, statusService, productImagesService); });


        }
        #region Commands
        private readonly RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (new RelayCommand(obj =>
                {

                    App.Current.MainWindow.Close();
                    for (int i = 0; i < App.Current.Windows.Count; i++)
                        App.Current.Windows[i].Close();
                }));
            }
        }
        private readonly RelayCommand _signOut;
        public RelayCommand SignOut
        {
            get
            {
                return _signOut ?? (new RelayCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите выйти?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //AuthorizationView authWindow = new AuthorizationView();
                        //authWindow.Show();
                        App.Current.MainWindow.Close();
                    }
                }));
            }
        }
        #endregion
        private BitmapImage ConvertByteArrayToBitMapImage(byte[] imageByteArray)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(imageByteArray))
            {
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = memStream;
                img.EndInit();
                img.Freeze();
            }
            return img;
        }
    }
}
