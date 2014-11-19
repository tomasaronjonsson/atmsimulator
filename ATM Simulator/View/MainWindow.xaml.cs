using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Map;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void VectorLayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //GeoPoint geoPt = this.theMap.Layers[0].ScreenToGeoPoint(e.GetPosition(this.theMap));
        }
    }
}
