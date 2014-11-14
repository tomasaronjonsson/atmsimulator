using System.Windows;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //todo review
        //The Remove track Window
        private void Track_RemoveWindow(object sender, RoutedEventArgs e)
        {
            Track_RemoveWindow d = new Track_RemoveWindow();
            d.Show();
        }

        //The Edit track Window
        private void Track_EditWindow(object sender, RoutedEventArgs e)
        {
            Track_EditWindow d = new Track_EditWindow();
            d.Show();
        }
    }
}
