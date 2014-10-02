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
using System.ServiceModel;
using ATMS_Server;
using ATMS_Client.ServiceReference1;
using System.Threading;
using ATMS_Client.ViewModels;

namespace ATMS_Client.Views
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : Window
    {
        private BrushConverter converter = new BrushConverter();
        string textColor = "white";

        public View()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }

        private void okService(object sender, TextChangedEventArgs e)
        {
            if (serviceBox.Text == "OK")
            {
                serviceBox.Background = Brushes.Green;
                serviceBox.Foreground = converter.ConvertFromString(textColor) as Brush;
            }

            else if (serviceBox.Text != "test")
            {
                serviceBox.Foreground = converter.ConvertFromString(textColor) as Brush;
                serviceBox.Background = Brushes.Red;
            }
        }

        private void okCallback(object sender, TextChangedEventArgs e)
        {
            if (callbackBox.Text.Contains("OK"))
            {
                callbackBox.Background = Brushes.Green;
                callbackBox.Foreground = converter.ConvertFromString(textColor) as Brush;
            }

            else if (callbackBox.Text != null)
            {
                callbackBox.Foreground = converter.ConvertFromString(textColor) as Brush;
                serviceBox.Background = Brushes.Red;
            }
        }

    }
}
