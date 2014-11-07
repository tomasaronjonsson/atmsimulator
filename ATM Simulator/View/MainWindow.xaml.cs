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

            /*
             *  Testing map
            string s = "ATB COL=255,255,255 STYLE=SOLID SIZE=1 FILLSTYLE=NULLC (#57.5031667 10.2193889 C )#57.5038611 10.2393333";

            foreach (Dse.MapFile.Shape item in MapFileParser.Parse(s).Shapes) 
            {
                mapcanvas.Children.Add(item);  // see HERE - this is not OAK :))
            }*/
        }

        private void Slider_Drop(object sender, DragEventArgs e)
        {

        }

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
