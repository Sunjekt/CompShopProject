using System.Windows;
using System.Windows.Controls;

namespace CompShopProject.View
{
    /// <summary>
    /// Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class ProductsView : UserControl
    {
        public ProductsView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            categoriesList.SelectedIndex = 0;
            producersList.SelectedIndex = 0;
        }

    }
}
