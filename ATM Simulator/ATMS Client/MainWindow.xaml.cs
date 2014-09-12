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
using ATMS_Client.ServiceReference1;

namespace ATMS_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServerInterfaceClient c1;

        public MainWindow()
        {
            InitializeComponent();
            this.c1 = new ServerInterfaceClient();
        }

        private void PokeServer_Click(object sender, RoutedEventArgs e)
        {
            resultBox.Text = c1.ReturnPoke();
        }
    }
}
