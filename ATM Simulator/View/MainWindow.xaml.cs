using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Map;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModel;

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
            GeoPoint geoPt = this.theMap.Layers[0].ScreenToGeoPoint(e.GetPosition(this.theMap));

            ContextMenu contextmenu = new ContextMenu();
            theMap.ContextMenu = contextmenu;

            MenuItem mi = new MenuItem();
            mi.Header = "Create Plot";
            mi.MouseLeftButtonDown += new MouseButtonEventHandler(createPlot);
            contextmenu.Items.Add(mi);
        }

        private void createPlot(object sender, MouseButtonEventArgs e)
        {
            /*var vm = (SimulationViewModel)DataContext;

            if (vm.CreateNewPlot.CanExecute(vm.tracks.Count > 0))
            {
                vm.CreateNewPlot.Execute(null);
            }*/
        }
    }
}
