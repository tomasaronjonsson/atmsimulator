using Dse.MapFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
