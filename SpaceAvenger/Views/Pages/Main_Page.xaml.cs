using System.Windows;
using System.Windows.Controls;

namespace SpaceAvenger.Views.Pages
{
    /// <summary>
    /// Interaction logic for Main_Page.xaml
    /// </summary>
    public partial class Main_Page : Page
    {
        public Main_Page()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Зроблено студентом 3 ого курсу Литвиновом Б Ю 135 гр",
                "About Author", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
