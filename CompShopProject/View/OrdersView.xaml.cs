using System.Windows;
using System.Windows.Controls;

namespace CompShopProject.View
{
    public partial class OrdersView : UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            yearsList.SelectedIndex = 0;
            monthsList.SelectedIndex = 0;
            statusesList.SelectedIndex = 0;
        }
    }
}