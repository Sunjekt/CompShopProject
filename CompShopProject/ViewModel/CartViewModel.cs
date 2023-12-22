using CompShopProject.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;
using CompShopProject.View;

namespace CompShopProject.ViewModel
{
    internal class CartViewModel : ObservableObject
    {
        private User _currentUser;
        private readonly IProductImagesService productImagesService;
        private readonly ICartItemsService cartItemsService;
        private readonly IOrdersService ordersService;

        public ObservableCollection<CartItem> Products { get; set; }

        private CartItem _selectedProduct;
        private string _starRatesImageSource;

        public CartViewModel(User currentUser, IProductImagesService productImagesService, ICartItemsService cartItemsService, IOrdersService ordersService)
        {
            this.productImagesService = productImagesService;
            this.cartItemsService = cartItemsService;
            this.ordersService = ordersService;

            _selectedProduct = new CartItem();
            _currentUser = currentUser;

            Products = new ObservableCollection<CartItem>();

            LoadCartItems();
        }

        #region Selected objects
        public CartItem SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                var buff_pathes = LoadImagesForProduct(_selectedProduct.Product.Id);
                SelectedProduct.Product.ProductImage = buff_pathes.ToList();
                if (SelectedProduct != null)
                {
                    CartProductView cartProductView = new CartProductView(SelectedProduct, _currentUser, cartItemsService);
                    cartProductView.ShowDialog();
                    _selectedProduct = null;
                }
                OnPropertyChanged("SelectedProduct");
            }
        }

        public int CartTotal
        {
            get { return _currentUser.CartPrice; }
            set
            {
                _currentUser.CartPrice = value;
                OnPropertyChanged("Total");
            }
        }

        public int CartQuantity
        {
            get { return _currentUser.CartQuantity; }
            set
            {
                _currentUser.CartQuantity = value;
                OnPropertyChanged("Total");
            }
        }
        #endregion

        #region Load data adapter
        private void LoadCartItems()
        {
            Products.Clear();
            _currentUser.CartQuantity = 0;
            _currentUser.CartPrice = 0;
            var products = cartItemsService.GetCartItemsByUserId(_currentUser.Id);
            foreach (var product in products)
            {
                Products.Add(product);
                product.Subtotal = product.Quantity * product.Product.Price;
                _currentUser.CartQuantity += product.Quantity;
                _currentUser.CartPrice += product.Subtotal;
                var images = LoadImagesForProduct(product.Product.Id);
                if (images.Count > 0)
                {
                    product.Product.ProductImage = images;
                    product.Product.ImageBytes = product.Product.ProductImage.ToList()[0].Image;
                }
                string rateImageSource = "";
                switch (product.Product.Rate)
                {
                    case 0:
                        rateImageSource = "/Images/StarRates/Star_rating_0_of_5.png";
                        break;
                    case 0.5:
                        rateImageSource = "/Images/StarRates/Star_rating_0.5_of_5.png";
                        break;
                    case 1:
                        rateImageSource = "/Images/StarRates/Star_rating_1_of_5.png";
                        break;
                    case 1.5:
                        rateImageSource = "/Images/StarRates/Star_rating_1.5_of_5.png";
                        break;
                    case 2:
                        rateImageSource = "/Images/StarRates/Star_rating_2_of_5.png";
                        break;
                    case 2.5:
                        rateImageSource = "/Images/StarRates/Star_rating_2.5_of_5.png";
                        break;
                    case 3:
                        rateImageSource = "/Images/StarRates/Star_rating_3_of_5.png";
                        break;
                    case 3.5:
                        rateImageSource = "/Images/StarRates/Star_rating_3.5_of_5.png";
                        break;
                    case 4:
                        rateImageSource = "/Images/StarRates/Star_rating_4_of_5.png";
                        break;
                    case 4.5:
                        rateImageSource = "/Images/StarRates/Star_rating_4.5_of_5.png";
                        break;
                    case 5:
                        rateImageSource = "/Images/StarRates/Star_rating_5_of_5.png";
                        break;
                }
                product.Product.CurrentRateSource = rateImageSource;
            }
        }

        private List<ProductImage> LoadImagesForProduct(int productId)
        {
            return productImagesService.GetImagesByProductId(productId).ToList();
        }
        #endregion

        #region Star image source adapter & load images func
        public string StarRatesImageSource
        {
            get => _starRatesImageSource;
            set
            {
                _starRatesImageSource = value;
                OnPropertyChanged("StarRatesImageSource");
            }
        }
        #endregion

        private readonly RelayCommand _createOrder;
        public RelayCommand CreateOrder
        {
            get
            {
                return _createOrder ?? (new RelayCommand(obj =>
                {
                    bool isOver = false;
                    foreach (var cartItem in Products)
                    {
                        if (cartItem.Quantity > cartItem.Product.Quantity)
                        {
                            isOver = true;
                            MessageBox.Show($"Товара с названием {cartItem.Product.Name} в коризне ({cartItem.Quantity}) больше, чем на складе ({cartItem.Product.Quantity})!");
                            break;
                        }
                    }
                    if (!isOver)
                    {
                        Order order = new Order();
                        order.UserId = _currentUser.Id;
                        order.CreationDate = DateTime.Now;
                        order.StatusId = 1;
                        ordersService.AddOrder(order, _currentUser.Id);

                        MessageBox.Show($"Заказ был успешно создан!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }));
            }
        }
    }
}
