using ATMS_Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Map;
using DevExpress.Map;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Threading;
using System.ComponentModel;
using System.Windows;

namespace ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        //storing a instance of the Simulation model
        SimulationModel model;



        public SimulationViewModel()
        {

            // initilization of the viewmodel 
            _viewModelCurrentTime = 0;
            _serverIsAvailable = false;
            _serverIsPlaying = false;
            _syncTimeWithServer = true;


            planes = null;
            historyPlanes = null;

            //creating a new model
            model = new SimulationModel();

            //These listen to the brodcasted messages sent by the Messenger
            Messenger.Default.Register<Scenario>(this, handleScenarioUpdate);
            Messenger.Default.Register<int>(this, handleServerTimeUpdate);
            Messenger.Default.Register<bool>(this, handleBoolChanges);
            Messenger.Default.Register<Track>(this, "createTrack", handleCreateTrack);
            Messenger.Default.Register<Track>(this, "removeTrack", handleRemoveTrack);
            Messenger.Default.Register<Track>(this, "editTrack", handleEditTrack);

            //initialize the plots list
            _plots = new List<Plot>();
            //ininitlize the tracks list
            _tracks = new BindingList<ViewModelTrack>();

            //end of initilization of the viewmodel

            model.startUp();
        }


        #region properties

        private int _viewModelCurrentTime;
        public int viewModelCurrentTime
        {
            get { return _viewModelCurrentTime; }
            set
            {
                if (value != _viewModelCurrentTime)
                {
                    _viewModelCurrentTime = value;
                    //in the case it's not the same (we are adjusting the slider or something we disable the sync with the server
                    if (viewModelCurrentTime != serverCurrentTime)
                        syncTimeWithServer = false;

                    //update the currentTime on each track
                    foreach (ViewModelTrack track in tracks)
                    {
                        track.currentTime = value;
                    }
                    RaisePropertyChanged("viewModelCurrentTime");
                }
            }
        }

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

        private List<MapDot> _planes;
        public List<MapDot> planes
        {
            get { return _planes; }
            set
            {
                if (value != _planes)
                {
                    _planes = value;
                    RaisePropertyChanged("planes");
                }
            }
        }

        private List<MapDot> _historyPlanes;
        public List<MapDot> historyPlanes
        {
            get { return _historyPlanes; }
            set
            {
                if (value != _historyPlanes)
                {
                    _historyPlanes = value;
                    RaisePropertyChanged("historyPlanes");
                }
            }
        }

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

        //these hold the list of plots 
        private List<Plot> _plots;
        public List<Plot> plots
        {
            get { return _plots; }
            set
            {
                if (value != _plots)
                {
                    _plots = value;
                    RaisePropertyChanged("plots");
                }
            }
        }

        //these hold the list of tracks
        private BindingList<ViewModelTrack> _tracks;
        public BindingList<ViewModelTrack> tracks
        {
            get { return _tracks; }
            set
            {
                if (value != _tracks)
                {
                    //we have to copy the list else, the binding will not update 
                    _tracks = value;
                    RaisePropertyChanged("tracks");
                }
            }
        }

        //property to store information about the selected plane og track
        private ViewModelTrack _selectedTrack;
        public ViewModelTrack selectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                if (value != _selectedTrack)
                {
                    _selectedTrack = value;
                    RaisePropertyChanged("selectedTrack");
                }
            }
        }

        #endregion


        #region RelayCommands
        //this is the create scenario command that calls the create scenario method from the model
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
                           return serverIsAvailable && !serverIsPlaying;
                       });
                }
                return _PlaySimulation;
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
                           return serverIsAvailable;
                       });
                }
                return _CreateNewTrack;
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
                           MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wanna remove track with callsign " + selectedTrack.callsign + "?", "Remove track Confirmation", System.Windows.MessageBoxButton.YesNo);
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
                           //         await model.edit(selectedTrack.toTrack());
                       },
                       () =>
                       {
                           return serverIsAvailable && (selectedTrack != null);
                       });
                }
                return _EditTrack;
            }
        }

        #endregion
        #region Listening methods for the messenger

        //listens to the scenario time update
        private void handleServerTimeUpdate(int currentServertime)
        {
            serverCurrentTime = currentServertime;
            if (syncTimeWithServer)
            {
                viewModelCurrentTime = currentServertime;
            }
        }

        //listens to the scenario update
        private void handleScenarioUpdate(Scenario obj)
        {

            //check if the scenario is null
            if (obj != null)
            {
                //make a temp to store the new track list
                BindingList<ViewModelTrack> temp = new BindingList<ViewModelTrack>();
                //populate the track list
                foreach (Track t in obj.tracks)
                {
                    //converitng the base object to the derived version
                    temp.Add(new ViewModelTrack(t));
                }
                //storing the new list
                tracks = temp;
            }

        }
        /**
         * 
         * 
         * Purpose handle messenges sent from the Model when changes are made to a bool on the model and updating all boolean values of interest
         * */
        private void handleBoolChanges(bool obj)
        {
            //check if the model is null before doing anything
            if (model != null)
            {
                //the boolean values of interest
                serverIsPlaying = model.serverIsPlaying;
                serverIsAvailable = model.serverIsAvailable;
            }
        }

        /**
        * 
        * Purpose handle messenges sent from the Model when we add a track
        * */

        private void handleCreateTrack(Track t)
        {
            t.callsign = "test";
            tracks.Add(new ViewModelTrack(t));
        }

        /*
             * review Tomas - It looks like a simple solution to the fix. 
             **/
        private void handleRemoveTrack(Track t)
        {

            
            

            // List size -1 because we use RemoveAt(index)
            for (int i = tracks.Count - 1; i >= 0; i--)
            {
                if (tracks[i].trackID == t.trackID)
                {
                    tracks.RemoveAt(i);
                }
            }
        }

        private void handleEditTrack(Track t)
        {
            foreach (ViewModelTrack track in tracks)
                if (track.trackID == track.trackID)
                    track.edit(t);
        }

        #endregion


        private void populatePLanes()
        {
            List<MapDot> tempPlaneList = new List<MapDot>();
            List<MapDot> tempHistoryPLaneList = new List<MapDot>();




            foreach (Plot p in plots)
            {
                MapDot temp = new MapDot();
                GeoPoint geo = new GeoPoint();
                //todo fix
                temp.Size = 20;
                geo.Latitude = p.latitude;
                geo.Longitude = p.longitude;


                temp.Location = geo;
                tempPlaneList.Add(temp);

            }



            for (int i = 0; i < BuisnessLogicValues.numberOfHistoryPlots; i++)
            {

                int time = viewModelCurrentTime - ((i + 1) * BuisnessLogicValues.radarInterval);

                List<Plot> temp = null;
                if (model != null)
                    if (model.mainScenario != null)
                        temp = model.mainScenario.getNow(time);
                if (temp != null)
                    foreach (Plot t in temp.Where(x => x != null))
                    {
                        MapDot tempDot = new MapDot();
                        tempDot.Size = 10;

                        GeoPoint tempGeo = new GeoPoint();
                        tempGeo.Latitude = t.latitude;
                        tempGeo.Longitude = t.longitude;

                        tempDot.Location = tempGeo;

                        tempHistoryPLaneList.Add(tempDot);
                    }



            }
            historyPlanes = tempHistoryPLaneList;
            planes = tempPlaneList;

        }

    }
}
