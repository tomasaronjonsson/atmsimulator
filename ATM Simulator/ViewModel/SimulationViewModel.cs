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


namespace ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        SimulationModel model;

        //storing the global time in seconds




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

            Messenger.Default.Register<Track>(this, handleTrackChanges);

            //initialize the plots list
            _plots = new List<Plot>();
            //ininitlize the tracks list
            _tracks = new List<Track>();

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
                    if (viewModelCurrentTime != serverCurrentTime)
                        syncTimeWithServer = false;
                    plots = model.mainScenario.getNow(viewModelCurrentTime);
                    populatePLanes();

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
        private List<Track> _tracks;
        public List<Track> tracks
        {
            get { return _tracks; }
            set
            {
                if (value != _tracks)
                {
                    //we have to copy the list else, the binding will not update 
                    _tracks = value.Select(x => x).ToList() ;
                    RaisePropertyChanged("tracks");
                }
            }
        }

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
            get { return _RemoveTrack; }
            set
            {
                if (_RemoveTrack == null)
                {
                    _RemoveTrack = new RelayCommand(
                       async () =>
                       {                           
                           //todo
                           await model.createNewTrack();
                       },
                       () =>
                       {
                           return serverIsAvailable;
                       });
                }
                return _RemoveTrack;
            }
        }
        

        #endregion

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
            //create a temporary list to work on
            plots = obj.getNow(viewModelCurrentTime);
            //update the track list
            tracks = model.mainScenario.tracks;
            //todo
            populatePLanes();
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
        * Purpose handle messenges sent from the Model when changes are made to a track on the model
        * */

        private void handleTrackChanges(Track t)
        {
            
            tracks =  model.mainScenario.tracks;

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
