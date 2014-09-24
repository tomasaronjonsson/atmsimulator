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

namespace ATMS_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ServerInterfaceClient c1;

        public MainWindow()
        {
            InitializeComponent();

            // Construct InstanceContext to handle messages on callback interface
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler(updateClient));

            //create a client
            this.c1 = new ServerInterfaceClient(instanceContext);
        }

        //Click event method that pokes the server by calling a method which returns a simple string
        private void PokeServer_Click(object sender, RoutedEventArgs e)
        {
            resultBox.Text = c1.ReturnPoke();
        }
        //implement a metho  for the callback handler to manipulate the iunterface
        public void updateClient(string data)
        {
            if (Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => updateBox.Text = data);
            }
            else
            {
                updateBox.Text = data;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int ID = c1.RegisterClient(9999);

            updateBox.Text = ID.ToString();
        }
    }
}
