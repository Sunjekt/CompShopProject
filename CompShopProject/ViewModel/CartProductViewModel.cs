using CompShopProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;

namespace CompShopProject.ViewModel
{
    internal class CartProductViewModel : ObservableObject
    {
        private readonly ICartItemsService cartItemsService;
        private Product _product;
        private CartItem _cartItem;

        public List<double> RatesValues { get; set; }
        public CartProductViewModel() { }
        public CartProductViewModel(CartItem product, User currentUser, ICartItemsService cartItemsService)
        {
            this.cartItemsService = cartItemsService;

            _cartItem = product;

            _product = new Product()
            {
                Description = product.Product.Description,
                Quantity = product.Product.Quantity,
                CurrentRateSource = product.Product.CurrentRateSource,
                CreationDate = product.Product.CreationDate,
                Id = product.Product.Id,
                ImageBytes = product.Product.ImageBytes,
                Name = product.Product.Name,
                Pathes = product.Product.Pathes,
                Price = product.Product.Price,
                ProductImage = product.Product.ProductImage,
                Rate = product.Product.Rate,
                ProducerId = product.Product.ProducerId
            };

            _product.Pathes = new List<string>() { "/Images/Icons/defaultProductImage.png" };
        }

        #region Accessors
        public string[] Pathes
        {
            get { return _product.Pathes.ToArray(); }
            set
            {
                _product.Pathes = value;
                OnPropertyChanged("Pathes");
            }
        }
        public string Description
        {
            get { return _product.Description; }
            set
            {
                _product.Description = value;
                OnPropertyChanged("Description");
            }
        }
        public string Name
        {
            get { return _product.Name; }
            set
            {
                _product.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public int Price
        {
            get { return _product.Price; }
            set
            {
                _product.Price = value;
                OnPropertyChanged("Price");
            }
        }
        public int Quantity
        {
            get { return _cartItem.Quantity; }
            set
            {
                _cartItem.Quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        public double Rate
        {
            get { return _product.Rate; }
            set
            {
                bool isNormal = (_product.Rate >= 0 && _product.Rate < 6);
                if (!isNormal)
                    MessageBox.Show("Некорректный ввод рейтинга!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    _product.Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }
        public DateTime CreationDate
        {
            get { return DateTime.Now; }
            set
            {
                _product.CreationDate = DateTime.Now;
                OnPropertyChanged("CreationDate");
            }
        }
        #endregion

        #region Commands
        private readonly RelayCommand _saveChangesProductCommand;
        public RelayCommand SaveChangesProductCommand
        {
            get
            {
                return _saveChangesProductCommand ?? (new RelayCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите сохранить изменения?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        cartItemsService.UpdateCartItem(_cartItem);
                        MessageBox.Show($"{_product.Name} был успешно изменён!");
                    }
                }));
            }
        }
        private readonly RelayCommand _deleteProductCommand;
        public RelayCommand DeleteProductCommand
        {
            get
            {
                return _deleteProductCommand ?? (new RelayCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите убрать выбранный товар?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        cartItemsService.DeleteCartItemById(_cartItem.Id);
                        MessageBox.Show("Товар был успешно убран из коризны!");
                        App.Current.Windows[2].Close();
                    }
                }));
            }
        }
        #endregion
    }
}
