using System.Windows;
using System.Windows.Controls;

namespace CompShopProject.View
{
    /// <summary>
    /// Interaction logic for AddProducerView.xaml
    /// </summary>
    public partial class ProducerView : UserControl
    {
        public ProducerView()
        {
            InitializeComponent();
        }
        private void ProducerControl_Loaded(object sender, RoutedEventArgs e)
        {
            producersList.SelectedIndex = 0;
        }
    }
}
