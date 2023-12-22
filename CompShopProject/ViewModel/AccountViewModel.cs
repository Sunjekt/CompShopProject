using CompShopProject.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;
using CompShopProject.HashGenerators;
using CompShopProject.View;

namespace CompShopProject.ViewModel
{
    internal class AccountViewModel : ObservableObject
    {
        private const string _secret = "Sunjekt";
        private int attempts;

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

        public ObservableCollection<User> Users { get; set; }
        public AccountViewModel(ICartItemsService cartItemsService, ICategoriesService categoriesService, IOrderItemsService orderItemsService, IOrdersService ordersService, IProducersService producersService, IProductImagesService productImagesService, IProductsService productsService, IStatusService statusService, IUserImagesService userImagesService, IUsersService usersService)
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

            IsAdmin = false;
            attempts = 3;
        }

        #region Accessors
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        private string _secretPhrase;
        public string SecretPhrase
        {
            get { return _secretPhrase; }
            set
            {
                _secretPhrase = value;
                OnPropertyChanged(nameof(SecretPhrase));
            }
        }
        private bool _isAdmin;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                SecretPhrase = String.Empty;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }
        #endregion

        #region Commands
        private readonly RelayCommand _signIn;
        public RelayCommand SignIn
        {
            get
            {
                return _signIn ?? (new RelayCommand(async obj =>
                {
                    User user = usersService.FindUserByName(Name);
                    if (user != null && attempts > 0)
                    {

                        string pass = MD5Generator.ProduceMD5Hash(Password);

                        if (IsAdmin && SecretPhrase.ToLower().Trim() == "я" && user.Password == pass && user.Name == Name
                        || !IsAdmin && user.Name == Name && user.Password == pass)
                        {
                            if (user.Deleted_at == null)
                            {
                                MainWindow window = new MainWindow(user, cartItemsService, categoriesService, orderItemsService, ordersService, producersService, productImagesService, productsService, statusService, userImagesService, usersService);
                                window.Show();
                                App.Current.MainWindow = window;
                                App.Current.Windows[0].Close();
                            }

                            else
                            {
                                MessageBoxResult result = MessageBox.Show($"Этот аккаунт был удалён. Вы хотите восстановить его?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    user.Deleted_at = null;
                                    usersService.UpdateUser(user);
                                    MessageBox.Show("Ваш аккаунт успешно восстановлен!");

                                    MainWindow window = new MainWindow(user, cartItemsService, categoriesService, orderItemsService, ordersService, producersService, productImagesService, productsService, statusService, userImagesService, usersService);
                                    window.Show();
                                    App.Current.MainWindow = window;
                                    App.Current.Windows[0].Close();
                                }
                            }

                        }
                        else
                            MessageBox.Show($"Введены неверные данные! Осталось попыток: {attempts--}");
                    }
                    else
                    {
                        if (attempts > 0)
                            MessageBox.Show($"Пользователя с таким именем не найдено! Осталось попыток: {attempts--}");
                        else
                        {
                            MessageBox.Show("Попробуйте войти позже.");
                            App.Current.MainWindow.Close();
                        }
                    }
                }));
            }
        }
        private readonly RelayCommand _signUp;
        public RelayCommand SignUp
        {
            get
            {
                return _signUp ?? (new RelayCommand(async obj =>
                {
                    RegistrationView registrationView = new RegistrationView();
                    registrationView.ShowDialog();
                    registrationView.Focus();
                }));
            }
        }
        #endregion
    }
}
