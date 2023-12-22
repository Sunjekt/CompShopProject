using CompShopProject.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace CompShopProject.View
{
    /// <summary>
    /// Interaction logic for RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : Window
    {
        public RegistrationView()
        {
            InitializeComponent();
            this.DataContext = new RegistrationViewModel();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void crossButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
