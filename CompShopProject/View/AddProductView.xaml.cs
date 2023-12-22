using System.Windows;
using System.Windows.Controls;

namespace CompShopProject.View
{
    /// <summary>
    /// Interaction logic for AddProductView.xaml
    /// </summary>
    public partial class AddProductView : UserControl
    {
        public AddProductView()
        {
            InitializeComponent();
            //this.DataContext = new AddProductViewModel();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            producersList.SelectedIndex = 0;
            categoriesList.SelectedIndex = 0;
        }
    }
}
