using CompShopProject.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;

namespace CompShopProject.ViewModel
{
    internal class OrderDetailsViewModel : ObservableObject
    {
        private readonly IProductImagesService productImagesService;
        private readonly IOrdersService ordersService;
        private readonly IOrderItemsService orderItemsService;
        private readonly IStatusService statusService;

        private string _starRatesImageSource;

        public ObservableCollection<OrderItem> Products { get; set; }

        private Status _selectedStatus;
        private User _currentUser;
        private Order _order;

        public ObservableCollection<Status> Statuses { get; set; }
        public OrderDetailsViewModel() { }
        public OrderDetailsViewModel(Order order, User currentUser, IProductImagesService productImagesService, IOrdersService ordersService, IOrderItemsService orderItemsService, IStatusService statusService)
        {
            this.productImagesService = productImagesService;
            this.ordersService = ordersService;
            this.orderItemsService = orderItemsService;
            this.statusService = statusService;

            _selectedStatus = new Status();
            _currentUser = currentUser;

            Statuses = new ObservableCollection<Status>();
            Products = new ObservableCollection<OrderItem>();

            _order = new Order();
            _order.Id = order.Id;
            _order.UserId = order.UserId;
            _order.CreationDate = order.CreationDate;
            _order.Status = order.Status;
            _order.StatusId = order.StatusId;

            LoadOrderItems();
            LoadStatuses();
        }

        public string ActualStatus
        {
            get { return _order.Status.Name; }
            set
            {
                _order.Status.Name = value;
                OnPropertyChanged("ActualStatus");
            }
        }

        #region Selected objects
        public Status SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                if (SelectedStatus != null)
                    _order.StatusId = SelectedStatus.Id;
                OnPropertyChanged("SelectedStatus");
            }
        }
        #endregion

        #region Load data adapter
        private void LoadStatuses()
        {
            Statuses.Clear();
            var statuses = statusService.GetAllStatuses();
            foreach (var status in statuses)
                Statuses.Add(status);
        }

        private void LoadOrderItems()
        {
            Products.Clear();
            var products = orderItemsService.GetOrderItemsByOrderId(_order.Id);
            foreach (var product in products)
            {
                Products.Add(product);
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

        #region Commands
        private readonly RelayCommand _changeStatus;
        public RelayCommand ChangeStatus
        {
            get
            {
                return _changeStatus ?? (new RelayCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите сохранить изменения?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (_order.StatusId == 3)
                            ordersService.CancelOrder(_order);
                        ordersService.UpdateOrder(_order);
                        MessageBox.Show($"Заказ был успешно изменён!");
                    }
                }));
            }
        }

        private readonly RelayCommand _cancelOrder;
        public RelayCommand CancelOrder
        {
            get
            {
                return _cancelOrder ?? (new RelayCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите отменить заказ?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _order.StatusId = 3;
                        ordersService.CancelOrder(_order);
                        ordersService.UpdateOrder(_order);
                        MessageBox.Show($"Заказ был успешно отменен!");
                    }
                }));
            }
        }
        #endregion
    }
}
