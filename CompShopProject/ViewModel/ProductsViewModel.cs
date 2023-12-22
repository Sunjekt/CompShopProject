using CompShopProject.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Interfaces.Services;
using DomainModel.Models;
using CompShopProject.View;

namespace CompShopProject.ViewModel
{
    internal class ProductsViewModel : ObservableObject
    {
        private User _currentUser;
        private readonly IProductsService productsService;
        private readonly IProducersService producersService;
        private readonly ICategoriesService categoriesService;
        private readonly IProductImagesService productImagesService;
        private readonly ICartItemsService cartItemsService;

        public ObservableCollection<Producer> Producers { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        private Category _selectedCategory;
        private Producer _selectedProducer;
        private Product _selectedProduct;

        private string _starRatesImageSource;
        public ProductsViewModel(User currentUser, IProductsService productsService, IProducersService producersService, ICategoriesService categoriesService, IProductImagesService productImagesService, ICartItemsService cartItemsService)
        {
            this.productsService = productsService;
            this.producersService = producersService;
            this.categoriesService = categoriesService;
            this.productImagesService = productImagesService;
            this.cartItemsService = cartItemsService;


            _selectedProducer = new Producer();
            _selectedCategory = new Category();
            _selectedProduct = new Product();
            _currentUser = currentUser;

            Producers = new ObservableCollection<Producer>();
            Categories = new ObservableCollection<Category>();
            Products = new ObservableCollection<Product>();

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
                    LoadProductsByProducerAndCategory(SelectedProducer.Id, SelectedCategory.Id);
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
                    LoadProductsByProducerAndCategory(SelectedProducer.Id, SelectedCategory.Id);
                OnPropertyChanged("SelectedProducer");
            }
        }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                var buff_pathes = LoadImagesForProduct(_selectedProduct.Id);
                SelectedProduct.ProductImage = buff_pathes.ToList();
                if (SelectedProduct != null)
                {
                    ProductDetailsView productDetailsView = new ProductDetailsView(SelectedProduct, _currentUser, categoriesService, producersService, productsService, cartItemsService);
                    productDetailsView.ShowDialog();
                    _selectedProduct = null;
                }
                OnPropertyChanged("SelectedProduct");
            }
        }

        #endregion

        #region Load data adapter
        private void LoadProducers()
        {
            Producers.Clear();
            Producers.Add(new Producer() { Id = -2, Name = "Все производители", Rate = -1 });
            var producers = producersService.GetAllProducers();
            foreach (var producer in producers)
                Producers.Add(producer);
        }
        private void LoadCategories()
        {
            Categories.Clear();
            Categories.Add(new Category() { Id = -2, Name = "Все категории", Popularity = -1 });
            var categories = categoriesService.GetAllCategories();
            foreach (var category in categories)
                Categories.Add(category);
        }
        private void LoadProducts()
        {
            Products.Clear();
            List<Product> products;
            if (_currentUser.RoleId == 1)
                products = productsService.GetAllProducts();
            else
                products = productsService.GetAllExistingProducts();
            foreach (var product in products)
            {
                Products.Add(product);
                if (product.Deleted_at != null)
                    product.Color = "Red";
                else
                    product.Color = "DarkOrange";
                var images = LoadImagesForProduct(product.Id);
                if (images.Count > 0)
                {
                    product.ProductImage = images;
                    product.ImageBytes = product.ProductImage.ToList()[0].Image;
                }
                string rateImageSource = "";
                switch (product.Rate)
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
                product.CurrentRateSource = rateImageSource;
            }
        }
        private void LoadProductsByProducerAndCategory(int producerId, int categoryId)
        {
            Products.Clear();
            if (producerId != -2 && categoryId != -2)
            {
                List<Product> products;
                if (_currentUser.RoleId == 1)
                {
                    products = productsService.GetProductsByProducerAndCategoryId(producerId, categoryId);
                }
                else
                {
                    products = productsService.GetExistingProductsByProducerAndCategoryId(producerId, categoryId);
                }
                foreach (var product in products)
                {
                    Products.Add(product);
                    if (product.Deleted_at != null)
                        product.Color = "Red";
                    else
                        product.Color = "DarkOrange";
                    var images = LoadImagesForProduct(product.Id);
                    if (images.Count > 0)
                    {
                        product.ProductImage = images;
                        product.ImageBytes = product.ProductImage.ToList()[0].Image;
                        string rateImageSource = "";
                        switch (product.Rate)
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
                        product.CurrentRateSource = rateImageSource;
                    }
                }
                OnPropertyChanged("Products");
            }
            else if (producerId == -2 && categoryId != -2)
            {
                List<Product> products;
                if (_currentUser.RoleId == 1)
                {
                    products = productsService.GetProductsByCategoryId(categoryId);
                }
                else
                {
                    products = productsService.GetExistingProductsByCategoryId(categoryId);
                }
                foreach (var product in products)
                {
                    Products.Add(product);
                    if (product.Deleted_at != null)
                        product.Color = "Red";
                    else
                        product.Color = "DarkOrange";
                    var images = LoadImagesForProduct(product.Id);
                    if (images.Count > 0)
                    {
                        product.ProductImage = images;
                        product.ImageBytes = product.ProductImage.ToList()[0].Image;
                        string rateImageSource = "";
                        switch (product.Rate)
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
                        product.CurrentRateSource = rateImageSource;
                    }
                }
                OnPropertyChanged("Products");
            }
            else if (producerId != -2 && categoryId == -2)
            {
                List<Product> products;
                if (_currentUser.RoleId == 1)
                {
                    products = productsService.GetProductsByProducerId(producerId);
                }
                else
                {
                    products = productsService.GetExistingProductsByProducerId(producerId);
                }
                foreach (var product in products)
                {
                    Products.Add(product);
                    if (product.Deleted_at != null)
                        product.Color = "Red";
                    else
                        product.Color = "DarkOrange";
                    var images = LoadImagesForProduct(product.Id);
                    if (images.Count > 0)
                    {
                        product.ProductImage = images;
                        product.ImageBytes = product.ProductImage.ToList()[0].Image;
                        string rateImageSource = "";
                        switch (product.Rate)
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
                        product.CurrentRateSource = rateImageSource;
                    }
                }
                OnPropertyChanged("Products");
            }
            else
                LoadProducts();
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

        #region Commands
        private readonly RelayCommand _refreshCategories;
        public RelayCommand RefreshCategories
        {
            get
            {
                return _refreshCategories ?? (new RelayCommand(obj =>
                {
                    LoadCategories();
                    if (Categories.Count > 0)
                        SelectedCategory = Categories[0];
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
                    if (Producers.Count > 0)
                        SelectedProducer = Producers[0];
                }));
            }
        }
        #endregion
    }
}
