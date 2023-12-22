using System.Windows;
using System.Windows.Controls;

namespace CompShopProject.View
{
    /// <summary>
    /// Interaction logic for AddCategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        public CategoryView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            categoriesList.SelectedIndex = 0;
        }
    }
}
