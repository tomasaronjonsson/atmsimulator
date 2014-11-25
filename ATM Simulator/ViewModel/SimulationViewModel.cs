using ATMS_Model;
using DevExpress.Xpf.Map;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Model;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using DevExpress.Xpf.Map;
using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        // Store an instance of the model
        SimulationModel model;

        //at the moment we only have a static value for the map
        string mapPath = "C:/maps.xml";

        List<MapImporter.MapObject> mapObjects;

        bool newTrackCreated;

        #region Properties

        //store the mapitem list that represents Inseros map
        private ObservableCollection<MapItem> _map;
        public ObservableCollection<MapItem> map
        {
            get { return _map; }
            set
            {
                if (value != _map)
                {
                    _map = value;
                    RaisePropertyChanged("map");
                }
            }
        }


        //Stores the current time
        private int _viewModelCurrentTime;
        public int viewModelCurrentTime
        {
            get { return _viewModelCurrentTime; }
            set
            {
                if (value != _viewModelCurrentTime)
                {
                    _viewModelCurrentTime = value;

                    // Lower the timer synchronization flag if times are not identical
                    if (viewModelCurrentTime != serverCurrentTime)
                        syncTimeWithServer = false;

                    // Update the current time on each track
                    foreach (ViewModelTrack track in tracks)
                    {
                        track.currentTime = value;
                    }
                    RaisePropertyChanged("viewModelCurrentTime");
                }
            }
        }

        // A flag that shows if the client is in sync with the server
        private bool _syncTimeWithServer;
        public bool syncTimeWithServer
        {
            get { return _syncTimeWithServer; }
            set
            {
                if (value != _syncTimeWithServer)
                {
                    _syncTimeWithServer = value;
                    RaisePropertyChanged("syncTimeWithServer");
                }
            }
        }

        // Stores the server time
        private int _serverCurrentTime;
        public int serverCurrentTime
        {
            get { return _serverCurrentTime; }
            set
            {
                if (value != _serverCurrentTime)
                {
                    _serverCurrentTime = value;
                    RaisePropertyChanged("serverCurrentTime");
                }
            }
        }

        // A flag that shows if the server is playing
        private bool _serverIsPlaying;
        public bool serverIsPlaying
        {
            get { return _serverIsPlaying; }
            set
            {
                if (value != _serverIsPlaying)
                {
                    _serverIsPlaying = value;
                    if (_PlaySimulation != null)
                        _PlaySimulation.RaiseCanExecuteChanged();
                    RaisePropertyChanged("serverIsPlaying");
                }
            }
        }

        // A flag that shows if the server is available for request
        private bool _serverIsAvailable;
        public bool serverIsAvailable
        {
            get { return _serverIsAvailable; }
            set
            {
                if (value != _serverIsAvailable)
                {
                    _serverIsAvailable = value;
                    if (_CreateScenario != null)
                        _CreateScenario.RaiseCanExecuteChanged();
                    if (_PlaySimulation != null)
                        _PlaySimulation.RaiseCanExecuteChanged();
                    RaisePropertyChanged("serverIsAvilable");
                }
            }
        }

        // Stores the list of Tracks
        private BindingList<ViewModelTrack> _tracks;
        public BindingList<ViewModelTrack> tracks
        {
            get { return _tracks; }
            set
            {
                if (value != _tracks)
                {
                    _tracks = value;
                    RaisePropertyChanged("tracks");
                }
            }
        }

        // Stores the selected track & updates the current plot
        private ViewModelTrack _selectedTrack;
        public ViewModelTrack selectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                if (value != _selectedTrack)
                {

                    if (value != null)
                    {
                        _selectedTrack = value;

                        // Update the current plot
                        selectedPlot = _selectedTrack.currentPlot;
                        RaisePropertyChanged("selectedTrack");
                    }
                }
            }
        }

        // Stores the selected plot
        private ViewModelPlot _selectedPlot;
        public ViewModelPlot selectedPlot
        {
            get { return _selectedPlot; }
            set
            {
                if (value != _selectedPlot)
                {
                    _selectedPlot = value;
                    RaisePropertyChanged("selectedPlot");
                }
            }
        }

        private GeoPoint _newPlotLocation;
        public GeoPoint newPlotLocation
        {
            get { return _newPlotLocation; }
            set
            {
                if (value != _newPlotLocation)
                {
                    _newPlotLocation = value;
                    RaisePropertyChanged("newPlotLocation");
                }
            }
        }

        #endregion


        /*
         * The ViewModel constructor includes
         * 
         * -the initialization of the properties and variables
         * -the registration of the messages sent by the model
         * -the call for the startUp method on the model
         * */
        public SimulationViewModel()
        {
            //Current time starts from 0
            _viewModelCurrentTime = 0;

            //Server is not available until the model starts up
            _serverIsAvailable = false;

            //Server is not playing upon the start of the program
            _serverIsPlaying = false;

            //Client current time is in sync with the server time at the start of the program
            _syncTimeWithServer = true;

            //newly created track flag
            newTrackCreated = false;

            //Track list is initialized
            _tracks = new BindingList<ViewModelTrack>();

            //Initialize the model
            model = new SimulationModel();

            //Register the possible incoming message from the model
            Messenger.Default.Register<int>(this, "serverTime", handleServerTimeUpdate);
            Messenger.Default.Register<bool>(this, "serverAvailability", checkIfServerIsAvailable);
            Messenger.Default.Register<bool>(this, "serverIsPlaying", checkIfServerIsPlaying);
            Messenger.Default.Register<Scenario>(this, "newScenario", handleNewScenario);
            Messenger.Default.Register<Track>(this, "createTrack", handleCreateTrack);
            Messenger.Default.Register<Track>(this, "removeTrack", handleRemoveTrack);
            Messenger.Default.Register<Track>(this, "editTrack", handleEditTrack);
            Messenger.Default.Register<Plot>(this, "createPlot", handleCreatePlot);
            Messenger.Default.Register<Plot>(this, "removePlot", handleRemovePlot);
            Messenger.Default.Register<Plot>(this, "editPlot", handleEditPlot);

            //Start up the model
            model.startUp();
        }

        /*
         * The relay commands are sent towards the Model 
         * 
         * They consist of two parts:
         * -the command itself
         * -the availability condition (the condition that needs to be met for the command to be available)
         * 
         * */
        #region RelayCommands

        private RelayCommand _ImportMap;
        public RelayCommand ImportMap
        {
            get
            {
                if (_ImportMap == null)
                {
                    _ImportMap = new RelayCommand(
                       () =>
                       {
                           importMap();
                       },
                       () =>
                       {
                           return serverIsAvailable;
                       });
                }

                return _ImportMap;
            }
        }

        private RelayCommand _CreateScenario;
        public RelayCommand CreateScenario
        {
            get
            {
                if (_CreateScenario == null)
                {
                    _CreateScenario = new RelayCommand(
                       async () =>
                       {
                           await model.createScenario();
                       },
                       () =>
                       {
                           return serverIsAvailable;
                       });
                }

                return _CreateScenario;
            }
        }

        private RelayCommand _PlaySimulation;
        public RelayCommand PlaySimulation
        {
            get
            {
                if (_PlaySimulation == null)
                {
                    _PlaySimulation = new RelayCommand(
                       async () =>
                       {
                           await model.playSimulation();
                           serverIsPlaying = true;
                       },
                       () =>
                       {
                           return serverIsAvailable && !serverIsPlaying && tracks.Count > 0;
                       });
                }
                return _PlaySimulation;
            }
        }

        private RelayCommand _PauseSimulation;
        public RelayCommand PauseSimulation
        {
            get
            {
                if (_PauseSimulation == null)
                {
                    _PauseSimulation = new RelayCommand(
                       async () =>
                       {
                           await model.pauseSimulation();
                           serverIsPlaying = false;
                       },
                       () =>
                       {
                           return serverIsAvailable && serverIsPlaying;
                       });
                }
                return _PauseSimulation;
            }
        }

        private RelayCommand _CreateNewTrack;
        public RelayCommand CreateNewTrack
        {
            get
            {
                if (_CreateNewTrack == null)
                {
                    _CreateNewTrack = new RelayCommand(
                       async () =>
                       {
                           await model.createNewTrack();
                       },
                       () =>
                       {
                           return serverIsAvailable && _tracks.Count != 0 && !serverIsPlaying;
                       });
                }
                return _CreateNewTrack;
            }
        }

        private RelayCommand _CreateNewTrackOnMap;
        public RelayCommand CreateNewTrackOnMap
        {
            get
            {
                if (_CreateNewTrackOnMap == null)
                {
                    _CreateNewTrackOnMap = new RelayCommand(
                       async () =>
                       {
                           newTrackCreated = true;
                           await model.createNewTrackOnMap(newPlotLocation.Latitude, newPlotLocation.Longitude);
                       },
                       () =>
                       {
                           return serverIsAvailable && _tracks.Count != 0 && !serverIsPlaying;
                       });
                }
                return _CreateNewTrackOnMap;
            }
        }

        private RelayCommand _RemoveTrack;
        public RelayCommand RemoveTrack
        {
            get
            {
                if (_RemoveTrack == null)
                {
                    _RemoveTrack = new RelayCommand(
                       async () =>
                       {
                           MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want remove track with callsign " + selectedTrack.callsign + "?", "Remove track Confirmation", System.Windows.MessageBoxButton.YesNo);
                           if (messageBoxResult == MessageBoxResult.Yes)
                               await model.removeTrack(selectedTrack.toTrack());
                       },
                       () =>
                       {
                           return serverIsAvailable && (selectedTrack != null);
                       });
                }
                return _RemoveTrack;
            }
        }

        private RelayCommand _EditTrack;
        public RelayCommand EditTrack
        {
            get
            {
                if (_EditTrack == null)
                {
                    _EditTrack = new RelayCommand(
                       async () =>
                       {
                           await model.editTrack(selectedTrack.toTrack());
                       },
                       () =>
                       {
                           return serverIsAvailable && (selectedTrack != null);
                       });
                }
                return _EditTrack;
            }
        }

        private RelayCommand _CreateNewPlot;
        public RelayCommand CreateNewPlot
        {
            get
            {
                if (_CreateNewPlot == null)
                {
                    _CreateNewPlot = new RelayCommand(
                       async () =>
                       {
                           await model.createNewPlot(selectedTrack.toTrack());
                       },
                       () =>
                       {
                           return serverIsAvailable && (selectedTrack != null) && !serverIsPlaying;
                       });
                }
                return _CreateNewPlot;
            }
        }

        private RelayCommand _AddWaypointToMap;
        public RelayCommand AddWaypointToMap
        {
            get
            {
                if (_AddWaypointToMap == null)
                {
                    _AddWaypointToMap = new RelayCommand(
                       async () =>
                       {
                           await model.addWaypointToMap(selectedTrack.toTrack(), newPlotLocation.Latitude, newPlotLocation.Longitude);
                       },
                       () =>
                       {
                           return serverIsAvailable && (selectedTrack != null) && !serverIsPlaying
;
                       });
                }
                return _AddWaypointToMap;
            }
        }

        private RelayCommand _RemovePlot;
        public RelayCommand RemovePlot
        {
            get
            {
                if (_RemovePlot == null)
                {
                    _RemovePlot = new RelayCommand(
                       async () =>
                       {
                           MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want remove plot at time " + selectedPlot.time + " belonging to callsign " + selectedTrack.callsign + "?", "Remove plot Confirmation", System.Windows.MessageBoxButton.YesNo);
                           if (messageBoxResult == MessageBoxResult.Yes)
                               await model.removePlot(selectedPlot.toPlot());
                       },
                       () =>
                       {
                           return serverIsAvailable && (selectedPlot != null) && !serverIsPlaying;
                       });
                }
                return _RemovePlot;
            }
        }

        private RelayCommand _EditPlot;
        public RelayCommand EditPlot
        {
            get
            {
                if (_EditPlot == null)
                {
                    _EditPlot = new RelayCommand(
                       async () =>
                       {
                           await model.editPlot(selectedPlot.toPlot());
                       },
                       () =>
                       {
                           return serverIsAvailable && (selectedPlot != null);
                       });
                }
                return _EditPlot;
            }
        }

        #endregion

        /*
         * These methods handle the messages sent by the model
         * */
        #region Message handling methods

        /* 
         * Handle the server time update
         * 
         * -validate the model
         * -sync the serverCurrentTime
         * -check the syncTimeWithServer flag
         * -sync the viewModelCurrentTime
         * */
        private void handleServerTimeUpdate(int currentServertime)
        {
            if (model != null)
            {
                serverCurrentTime = currentServertime;
                if (syncTimeWithServer)
                {
                    viewModelCurrentTime = currentServertime;
                }
            }
        }

        /*
         * Check if the server is available
         * 
         * -validate the model
         * -update the flag
         * */
        private void checkIfServerIsAvailable(bool obj)
        {
            if (model != null)
            {
                serverIsAvailable = model.serverIsAvailable;
            }
        }

        /*
         * Check if the server isplaying
         * 
         * -validate the model
         * -update the flag
         * */
        private void checkIfServerIsPlaying(bool obj)
        {
            if (model != null)
            {
                serverIsPlaying = model.serverIsPlaying;
            }
        }

        /* Populate the scenario with the incoming tracks
         * 
         * -validate the input
         * -add all the tracks to the list of tracks
         * */
        private void handleNewScenario(Scenario s)
        {
            if (s != null)
            {
                foreach (Track item in s.tracks)
                {
                    tracks.Add(new ViewModelTrack(item));
                }
            }
        }

        /*
         * Create a track
         * 
         * -validate the input
         * -add the new track
         * */
        private void handleCreateTrack(Track t)
        {
            if (t != null)
            {
                ViewModelTrack vmT = new ViewModelTrack(t);
                tracks.Add(new ViewModelTrack(t));

                if (newTrackCreated)
                {
                    selectedTrack = vmT;
                    newTrackCreated = false;
                }
            }
        }

        /*
         * Remove track
         * 
         * -validate the input
         * -remove the track
         * */
        private void handleRemoveTrack(Track t)
        {
            if (t != null)
            {
                tracks.Remove(new ViewModelTrack(t));
            }
        }

        /*
         * Edit track
         * 
         * -validate the input
         * -find the trackToBeEdited
         * -validate the trackToBeEdited
         * -edit the track
         * */
        private void handleEditTrack(Track t)
        {
            if (t != null)
            {
                var trackToBeEdited = tracks.First(x => x.Equals(t));

                if (trackToBeEdited != null)
                    trackToBeEdited.edit(t);
            }
        }

        /* Create a new plot
         * 
         * -validate the input
         * -find the trackToAddTo
         * -validate the trackToAddTo
         * -convert the input to a ViewModelPlot
         * -add the plot
         * -select the plot
         * */
        private void handleCreatePlot(Plot p)
        {
            if (p != null)
            {
                var trackToAddInto = tracks.First(x => x.trackID == p.trackID);

                if (trackToAddInto != null)
                {
                    ViewModelPlot vmPlot = new ViewModelPlot(p);
                    trackToAddInto.plots.Add(vmPlot);
                    selectedPlot = vmPlot;
                }
            }
        }

        /*
         * Remove plot
         * 
         * -validate the input
         * -find the trackToLookInto
         * -validate the trackToLookInto
         * -remove the plot
         * */
        private void handleRemovePlot(Plot p)
        {
            if (p != null)
            {
                var trackToLookInto = tracks.First(x => x.trackID == p.trackID);

                if (trackToLookInto != null)
                    trackToLookInto.plots.Remove(new ViewModelPlot(p));
            }
        }

        /*
         * Edit a plot
         * 
         * -validate the input
         * -find the trackToLookInto
         * -validate the trackToLookInto
         * -find the plotToBeChanged
         * -validate the plotToBeChanged
         * -edit the plot
         * */
        private void handleEditPlot(Plot p)
        {
            if (p != null)
            {
                ViewModelTrack trackToLookInto = tracks.First(x => x.trackID == p.trackID);

                if (trackToLookInto != null)
                {
                    ViewModelPlot plotToBeChanged = trackToLookInto.plots.First(x => x.Equals(new ViewModelPlot(p)));

                    if (plotToBeChanged != null)
                        plotToBeChanged.edit(p);
                }
            }
        }

        #endregion

        /*
         * review alex 
         * 
         * import the map objects from insero and populate our map list
         */
        public void importMap()
        {

            //we are using the explicit names to not get them mixed with the devexpress or microsoft items
            //let's import the map
            List<MapImporter.MapObject> tempMapObjects = MapImporter.Parser.parse(mapPath);


            //make a tempoarary list of shape to extract from the map ( we are taking ALL maps in atm
            List<MapImporter.Shape> tempShapes = new List<MapImporter.Shape>();

            foreach (MapImporter.MapObject o in tempMapObjects)
            {
                foreach (MapImporter.Shape s in o.shapes)
                {
                    tempShapes.Add(s);
                }
            }


            //prepeare a list of mapitem it's a binding list because we are going to use it for the property
            List<MapItem> tempMap = new List<MapItem>();


            //change from insero's custom shape to a mapitem
            foreach (MapImporter.Shape s in tempShapes)
            {

                //chance their circle to devex mapdot
                if (s is MapImporter.Circle)
                {

                    MapImporter.Circle circle = (MapImporter.Circle)s;

                    MapDot tempDot = new MapDot();
                    tempDot.Size = circle.Radius;

                }
                //inseros polygon to devex polygon
                else if (s is MapImporter.Polygon)
                {
                    MapImporter.Polygon polygon = (MapImporter.Polygon)s;

                    MapPolygon tempPolygon = new MapPolygon();



                    for (int i = 0; i < polygon.Points.Count; i++)
                    {
                        GeoPoint newGeoPoint = new GeoPoint();

                        var split = polygon.Points[i].Split(',');


                        newGeoPoint.Latitude = Double.Parse(split[0], System.Globalization.CultureInfo.InvariantCulture);
                        newGeoPoint.Longitude = Double.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);

                        tempPolygon.Points.Add(newGeoPoint);
                    }
                    tempMap.Add(tempPolygon);
                }
                //inseros polyline to devex polyline
                else if (s is MapImporter.Polyline)
                {
                    MapImporter.Polyline polyline = (MapImporter.Polyline)s;


                    MapPolyline tempPolyline = new MapPolyline();

                    for (int i = 0; i < polyline.Points.Count; i++)
                    {
                        GeoPoint newGeoPoint = new GeoPoint();

                        var split = polyline.Points[i].Split(',');


                        newGeoPoint.Latitude = Double.Parse(split[0], System.Globalization.CultureInfo.InvariantCulture);
                        newGeoPoint.Longitude = Double.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);

                        tempPolyline.Points.Add(newGeoPoint);
                    }
                    tempMap.Add(tempPolyline);
                }


            }

            //store the map we have created
            map = new ObservableCollection<MapItem>(tempMap);
            //store the mapobjects for later use (save, selected wich ones to use tc.)
            mapObjects = tempMapObjects;

        }


    }
}
