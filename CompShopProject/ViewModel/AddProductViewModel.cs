﻿using CompShopProject.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;

namespace CompShopProject.ViewModel
{
    internal class AddProductViewModel : ObservableObject
    {
        private readonly IProductImagesService productImagesService;
        private readonly ICategoriesService categoriesService;
        private readonly IProducersService producersService;
        private readonly IProductsService productsService;

        private Product _product;
        private Category _selectedCategory;
        private Producer _selectedProducer;
        private string _starRatesImageSource;

        public List<double> RatesValues { get; set; }
        public ObservableCollection<Producer> Producers { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public AddProductViewModel(IProductImagesService productImagesService, ICategoriesService categoriesService, IProducersService producersService, IProductsService productsService)
        {
            this.productImagesService = productImagesService;
            this.categoriesService = categoriesService;
            this.producersService = producersService;
            this.productsService = productsService;


            _selectedCategory = new Category();
            _selectedProducer = new Producer();

            _product = new Product();

            _product.Pathes = new List<string>() { "/Images/Icons/defaultProductImage.png" }; // Default image for new product

            RatesValues = new List<double>() { 0, 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5 };

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
                OnPropertyChanged("SelectedCategory");
            }
        }
        public Producer SelectedProducer
        {
            get => _selectedProducer;
            set
            {
                _selectedProducer = value;
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
                    MessageBox.Show("Некорректный ввод!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    _product.Rate = value;
                    switch (value)
                    {
                        case 0:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_0_of_5.png";
                            break;
                        case 0.5:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_0.5_of_5.png";
                            break;
                        case 1:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_1_of_5.png";
                            break;
                        case 1.5:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_1.5_of_5.png";
                            break;
                        case 2:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_2_of_5.png";
                            break;
                        case 2.5:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_2.5_of_5.png";
                            break;
                        case 3:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_3_of_5.png";
                            break;
                        case 3.5:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_3.5_of_5.png";
                            break;
                        case 4:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_4_of_5.png";
                            break;
                        case 4.5:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_4.5_of_5.png";
                            break;
                        case 5:
                            StarRatesImageSource = "/Images/StarRates/Star_rating_5_of_5.png";
                            break;
                    }
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

        #region Commands
        private readonly RelayCommand _refreshCategories;
        public RelayCommand RefreshCategories
        {
            get
            {
                return _refreshCategories ?? (new RelayCommand(obj =>
                {
                    LoadCategories();
                }));
            }
        }
        private readonly RelayCommand _refreshProducers;
        public RelayCommand RefreshProducers
        {
            get
            {
                return _refreshProducers ?? (new RelayCommand(obj =>
                {
                    LoadProducers();
                }));
            }
        }
        private readonly RelayCommand _addProduct;
        public RelayCommand AddProduct
        {
            get
            {
                return _addProduct ?? (new RelayCommand(obj =>
                {
                    _product.CreationDate = DateTime.Now;
                    _product.ImageBytes = null;
                    _product.ProducerId = SelectedProducer.Id;
                    _product.CategoryId = SelectedCategory.Id;
                    productsService.AddNewProduct(_product);
                    if (_product.Pathes?.Count() > 0)
                    {
                        var dbProduct = productsService.GetProductByName(_product.Name);
                        if (dbProduct != null)
                            productImagesService.AddImages(Pathes, dbProduct.Id);
                    }
                    MessageBox.Show($"{_product.Name} был успешно добавлен!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }));
            }
        }
        private readonly RelayCommand _openFileDialog;
        public RelayCommand OpenFileDialog
        {
            get
            {
                return _openFileDialog ?? (new RelayCommand(obj =>
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = true;
                    ofd.Title = "Выберите фотографию товара";
                    ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.apng;*.avif;*.gif;*.jfif;*.pjpeg";
                    ofd.ShowDialog();
                    if (ofd.FileNames.Count() > 0 || ofd.FileName.Count() > 0)
                        Pathes = ofd.FileNames;
                }));
            }
        }
        #endregion
    }
}
