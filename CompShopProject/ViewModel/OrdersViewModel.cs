using CompShopProject.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;
using CompShopProject.View;
using System.Windows.Forms;
using System.Text;

namespace CompShopProject.ViewModel
{
    internal class OrdersViewModel : ObservableObject
    {
        private IOrdersService ordersService;
        private IOrderItemsService orderItemsService;
        private IProductImagesService productImagesService;
        private IStatusService statusService;

        private Order _selectedOrder;
        private string _selectedMonth;
        private string _selectedYear;
        private Status _selectedStatus;
        private int _selectedMonthIndex;

        private User _currentUser;

        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<string> Months { get; set; }
        public ObservableCollection<string> Years { get; set; }
        public ObservableCollection<Status> Statuses { get; set; }
        public OrdersViewModel(User currentUser, IOrdersService ordersService, IOrderItemsService orderItemsService, IStatusService statusService, IProductImagesService productImagesService)
        {
            _selectedMonthIndex = 0;

            this.productImagesService = productImagesService;
            this.ordersService = ordersService;
            this.orderItemsService = orderItemsService;
            this.statusService = statusService;

            _selectedOrder = new Order();
            _selectedStatus = new Status();
            _currentUser = currentUser;

            Orders = new ObservableCollection<Order>();
            Statuses = new ObservableCollection<Status>();

            if (_currentUser.Role.Name == "Администратор")
                AdminButtons = Visibility.Visible;
            else
                AdminButtons = Visibility.Collapsed;

            LoadStatuses();
            LoadMonths();
            LoadYears();
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
        public Status SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                LoadOrderByMonthAndYearAndStatus(SelectedStatus.Id, _selectedMonthIndex, SelectedYear);
                OnPropertyChanged("SelectedStatus");
            }
        }
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                _selectedMonthIndex = FindMonthIndex(_selectedMonth);
                LoadOrderByMonthAndYearAndStatus(SelectedStatus.Id, _selectedMonthIndex, SelectedYear);
                OnPropertyChanged("SelectedMonth");
            }
        }
        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                LoadOrderByMonthAndYearAndStatus(SelectedStatus.Id, _selectedMonthIndex, SelectedYear);
                OnPropertyChanged("SelectedYear");
            }
        }
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                if (SelectedOrder != null)
                {
                    OrderDetailsView orderDetailsView = new OrderDetailsView(SelectedOrder, _currentUser, productImagesService, ordersService, orderItemsService, statusService);
                    orderDetailsView.ShowDialog();
                }
                OnPropertyChanged("SelectedProduct");
            }
        }

        #region Load data adapter

        private void LoadStatuses()
        {
            Statuses.Clear();
            Statuses.Add(new Status() { Id = -2, Name = "Все статусы" });
            var statuses = statusService.GetAllStatuses();
            foreach (var status in statuses)
                Statuses.Add(status);
        }
        private void LoadMonths()
        {
            Months = new ObservableCollection<string>()
            {
                "Все месяцы", "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
            };
        }

        private void LoadYears()
        {
            Years = new ObservableCollection<string>();

            Years.Add("Все года");

            int currentYear = DateTime.Now.Year;
            for (int year = 2020; year <= currentYear; year++)
            {
                Years.Add(year.ToString());
            }
        }

        private void LoadOrders()
        {
            Orders.Clear();
            List<Order> orders;
            if (_currentUser.Id == 1)
                orders = ordersService.GetAllOrders();
            else
                orders = ordersService.GetOrdersByUserId(_currentUser.Id);
            foreach (var order in orders)
            {
                order.Statuses = statusService.GetAllStatuses();
                order.OrderPrice = 0;
                order.OrderQuantity = 0;
                Orders.Add(order);
                var orderItems = orderItemsService.GetOrderItemsByOrderId(order.Id);
                foreach (var orderItem in orderItems)
                {
                    order.OrderPrice += orderItem.Product.Price * orderItem.Quantity;
                    order.OrderQuantity += orderItem.Quantity;
                }
            }
        }

        private void LoadOrderByMonthAndYearAndStatus(int status, int month, string year)
        {
            Orders.Clear();
            if (month != 0)
            {
                if (year != "Все года")
                {
                    if (status != -2)
                    {
                        List<Order> orders;
                        orders = ordersService.GetOrdersByMonthAndYearAndStatus(status, month, Int32.Parse(year));

                        foreach (var order in orders)
                        {
                            order.OrderPrice = 0;
                            order.OrderQuantity = 0;
                            Orders.Add(order);
                            var orderItems = orderItemsService.GetOrderItemsByOrderId(order.Id);
                            foreach (var orderItem in orderItems)
                            {
                                order.OrderPrice += orderItem.Product.Price * orderItem.Quantity;
                                order.OrderQuantity += orderItem.Quantity;
                            }
                        }
                    }
                    else
                    {
                        List<Order> orders;
                        orders = ordersService.GetOrdersByMonthAndYear(month, Int32.Parse(year));

                        foreach (var order in orders)
                        {
                            order.OrderPrice = 0;
                            order.OrderQuantity = 0;
                            Orders.Add(order);
                            var orderItems = orderItemsService.GetOrderItemsByOrderId(order.Id);
                            foreach (var orderItem in orderItems)
                            {
                                order.OrderPrice += orderItem.Product.Price * orderItem.Quantity;
                                order.OrderQuantity += orderItem.Quantity;
                            }
                        }
                    }
                }
                else
                {
                    if (status != -2)
                    {
                        List<Order> orders;
                        orders = ordersService.GetOrdersByMonthAndStatus(status, month);

                        foreach (var order in orders)
                        {
                            order.OrderPrice = 0;
                            order.OrderQuantity = 0;
                            Orders.Add(order);
                            var orderItems = orderItemsService.GetOrderItemsByOrderId(order.Id);
                            foreach (var orderItem in orderItems)
                            {
                                order.OrderPrice += orderItem.Product.Price * orderItem.Quantity;
                                order.OrderQuantity += orderItem.Quantity;
                            }
                        }
                    }
                    else
                    {
                        List<Order> orders;
                        orders = ordersService.GetOrdersByMonth(month);

                        foreach (var order in orders)
                        {
                            order.OrderPrice = 0;
                            order.OrderQuantity = 0;
                            Orders.Add(order);
                            var orderItems = orderItemsService.GetOrderItemsByOrderId(order.Id);
                            foreach (var orderItem in orderItems)
                            {
                                order.OrderPrice += orderItem.Product.Price * orderItem.Quantity;
                                order.OrderQuantity += orderItem.Quantity;
                            }
                        }
                    }
                }
            }

            else
            {
                if (year != "Все года")
                {
                    if (status != -2)
                    {
                        List<Order> orders;
                        orders = ordersService.GetOrdersByYearAndStatus(status, Int32.Parse(year));

                        foreach (var order in orders)
                        {
                            order.OrderPrice = 0;
                            order.OrderQuantity = 0;
                            Orders.Add(order);
                            var orderItems = orderItemsService.GetOrderItemsByOrderId(order.Id);
                            foreach (var orderItem in orderItems)
                            {
                                order.OrderPrice += orderItem.Product.Price * orderItem.Quantity;
                                order.OrderQuantity += orderItem.Quantity;
                            }
                        }
                    }
                    else
                    {
                        List<Order> orders;
                        orders = ordersService.GetOrdersByYear(Int32.Parse(year));

                        foreach (var order in orders)
                        {
                            order.OrderPrice = 0;
                            order.OrderQuantity = 0;
                            Orders.Add(order);
                            var orderItems = orderItemsService.GetOrderItemsByOrderId(order.Id);
                            foreach (var orderItem in orderItems)
                            {
                                order.OrderPrice += orderItem.Product.Price * orderItem.Quantity;
                                order.OrderQuantity += orderItem.Quantity;
                            }
                        }
                    }
                }
                else
                {
                    if (status != -2)
                    {
                        List<Order> orders;
                        orders = ordersService.GetOrdersByStatus(status);

                        foreach (var order in orders)
                        {
                            order.OrderPrice = 0;
                            order.OrderQuantity = 0;
                            Orders.Add(order);
                            var orderItems = orderItemsService.GetOrderItemsByOrderId(order.Id);
                            foreach (var orderItem in orderItems)
                            {
                                order.OrderPrice += orderItem.Product.Price * orderItem.Quantity;
                                order.OrderQuantity += orderItem.Quantity;
                            }
                        }
                    }
                    else
                    {
                        LoadOrders();
                    }
                }
            }
        }
        #endregion

        private int FindMonthIndex(string month)
        {
            for (int i = 0; i < Months.Count; i++)
            {
                if (Months[i] == month)
                    return i;
            }
            return -1;
        }

        private readonly RelayCommand _makeReport;
        public RelayCommand MakeReport
        {
            get
            {
                return _makeReport ?? (new RelayCommand(obj =>
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "CSV файлы (*.csv)|*.csv";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                        {
                            writer.WriteLine("Дата создания; Заказчик; Количество товаров; Цена");
                            foreach (var order in Orders)
                            {
                                string time = order.CreationDate.ToString("dd.MM.yyyy HH:mm:ss");
                                writer.WriteLine(time + "; " + order.User.Name + "; " + order.OrderQuantity + "; " + order.OrderPrice);
                            }
                        }

                        System.Windows.MessageBox.Show("Файл CSV успешно сохранен!");
                    }
                }));
            }
        }
    }
}
