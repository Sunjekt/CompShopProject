using System.Windows;
using Interfaces.Services;
using DomainModel.Models;
using CompShopProject.ViewModel;

namespace CompShopProject.View
{

    public partial class OrderDetailsView : Window
    {
        private readonly IProductImagesService productImagesService;
        private readonly IOrdersService ordersService;
        private readonly IOrderItemsService orderItemsService;
        private readonly IStatusService statusService;
        public OrderDetailsView(Order selectedOrder, User currentUser, IProductImagesService productImagesService, IOrdersService ordersService, IOrderItemsService orderItemsService, IStatusService statusService)
        {
            InitializeComponent();
            this.productImagesService = productImagesService;
            this.ordersService = ordersService;
            this.orderItemsService = orderItemsService;
            this.statusService = statusService;
            this.DataContext = new OrderDetailsViewModel(selectedOrder, currentUser, productImagesService, ordersService, orderItemsService, statusService);

            if (currentUser.RoleId == 2)
            {
                statusList.Visibility = Visibility.Collapsed;
                changeStatusButton.Visibility = Visibility.Collapsed;

                if (selectedOrder.Status.Name == "Получен" || selectedOrder.Status.Name == "Отменён")
                    cancelOrderButton.Visibility = Visibility.Collapsed;
            }
            else
                cancelOrderButton.Visibility = Visibility.Collapsed;
        }
    }
}
