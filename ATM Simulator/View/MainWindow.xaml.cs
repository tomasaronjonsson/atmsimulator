using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Map;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        /*
         * This method creates a context menu on the map layer
         * if a track is selected.
         * */
        private void VectorLayer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Sets the data context for the following operations
            var vm = (SimulationViewModel)DataContext;

            if (vm.tracks.Count > 0)
            {
                //Create a GeoPoint with the coordinates of the pointer
                GeoPoint geoPt = this.theMap.Layers[0].ScreenToGeoPoint(e.GetPosition(this.theMap));
                //Set the newPlotLocation property on the ViewModel
                vm.newPlotLocation = geoPt;

                /*
                 * -Create the context menu
                 * -Assign the context menu to the map
                 * -Create a menu item
                 * -Set the header of the menu item
                 * -Bind a command to the menu item
                 * -Add the menu item to the context menu
                 * */
                ContextMenu contextMenu = new ContextMenu();
                theMap.ContextMenu = contextMenu;

                MenuItem addWaypoint = new MenuItem();
                addWaypoint.Header = "Add waypoint";
                addWaypoint.Command = vm.AddWaypointToMap;
                MenuItem addTrack = new MenuItem();
                addTrack.Header = "Add track";
                addTrack.Command = vm.CreateNewTrackOnMap;

                contextMenu.Items.Add(addWaypoint);
                contextMenu.Items.Add(addTrack);   
            }
        }
    }
}
