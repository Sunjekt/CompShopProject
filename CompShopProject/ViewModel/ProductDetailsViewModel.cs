using CompShopProject.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;

namespace CompShopProject.ViewModel
{
    internal class ProductDetailsViewModel : ObservableObject
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProducersService producersService;
        private readonly IProductsService productsService;
        private readonly ICartItemsService cartItemsService;

        private Product _product;
        private User _currentUser;
        private Category _selectedCategory;
        private Producer _selectedProducer;

        public List<double> RatesValues { get; set; }
        public ObservableCollection<Producer> Producers { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ProductDetailsViewModel() { }
        public ProductDetailsViewModel(Product product, User currentUser, ICategoriesService categoriesService, IProducersService producersService, IProductsService productsService, ICartItemsService cartItemsService)
        {
            this.categoriesService = categoriesService;
            this.producersService = producersService;
            this.productsService = productsService;
            this.cartItemsService = cartItemsService;

            _selectedCategory = new Category();
            _selectedProducer = new Producer();

            _product = new Product()
            {
                Description = product.Description,
                Quantity = product.Quantity,
                CurrentRateSource = product.CurrentRateSource,
                CreationDate = product.CreationDate,
                Id = product.Id,
                ImageBytes = product.ImageBytes,
                Name = product.Name,
                Pathes = product.Pathes,
                Price = product.Price,
                ProductImage = product.ProductImage,
                Rate = product.Rate,
                ProducerId = product.ProducerId
            };
            _currentUser = currentUser;

            _product.Pathes = new List<string>() { "/Images/Icons/defaultProductImage.png" }; // Default image for new product

            Producers = new ObservableCollection<Producer>();
            Categories = new ObservableCollection<Category>();

            // Loading producers and categories for dropdown lists
            LoadProducers();
            LoadCategories();
        }

        #region Selected objects
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                if (SelectedCategory != null)
                    _product.CategoryId = SelectedCategory.Id;
                OnPropertyChanged("SelectedCategory");
            }
        }
        public Producer SelectedProducer
        {
            get => _selectedProducer;
            set
            {
                _selectedProducer = value;
                if (SelectedProducer != null)
                    _product.ProducerId = SelectedProducer.Id;
                OnPropertyChanged("SelectedProducer");
            }
        }
        #endregion

        #region Load data adapter
        private void LoadProducers()
        {
            Producers.Clear();
            var producers = producersService.GetAllProducers();
            foreach (var producer in producers)
                Producers.Add(producer);
        }
        private void LoadCategories()
        {
            Categories.Clear();
            var categories = categoriesService.GetAllCategories();
            foreach (var category in categories)
                Categories.Add(category);
        }
        #endregion

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
            get { return _product.Quantity; }
            set
            {
                _product.Quantity = value;
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
                        if (SelectedProducer.Name != null && SelectedCategory.Name != null)
                        {
                            productsService.UpdateProduct(_product);
                            MessageBox.Show($"{_product.Name} был успешно изменён!");
                        }
                        else
                            MessageBox.Show("Выберите производителя и категорию!");
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
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить выбранный товар?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        cartItemsService.DeleteCartItemsByProductId(_product.Id);
                        productsService.DeleteProductById(_product.Id);
                        MessageBox.Show("Товар был успешно удалён!");
                        App.Current.Windows[2].Close();
                    }
                }));
            }
        }
        private readonly RelayCommand _addToCartCommand;
        public RelayCommand AddToCartCommand
        {
            get
            {
                return _addToCartCommand ?? (new RelayCommand(obj =>
                {
                    cartItemsService.AddCartItem(_currentUser.Id, _product.Id);
                    MessageBox.Show($"{_product.Name} был добавлен в корзину!");
                }));
            }
        }

        private readonly RelayCommand _recoverProductCommand;
        public RelayCommand RecoverProductCommand
        {
            get
            {
                return _recoverProductCommand ?? (new RelayCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите восстановить данный товар?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _product.Deleted_at = null;
                        productsService.UpdateProduct(_product);
                        MessageBox.Show($"{_product.Name} был успешно восстановлен!");
                    }
                }));
            }
        }
        #endregion
    }
}
