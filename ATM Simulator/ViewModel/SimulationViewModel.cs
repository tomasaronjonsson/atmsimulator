using ATMS_Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        SimulationModel model;

        //storing the global time in seconds
        public int viewModelCurrentTime;

        public SimulationViewModel()
        {
            model = new SimulationModel();
            viewModelCurrentTime = 0;


            //This listens to the brodcasted messages sent by the Messenger
            Messenger.Default.Register<Scenario>(this, handleScenarioUpdate);
            //This listens to the brodcasted messages sent by the Messenger
            Messenger.Default.Register<int>(this, handleServerTimeUpdate);

            //initialize the plots list
            _plots = new List<Plot>();
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
                           return model.serverIsAvailable;
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
                       },
                       () =>
                       {
                           return model.serverIsAvailable && !model.serverIsPlaying;
                       });
                }
                return _PlaySimulation;
            }
        }


        #endregion


        #region Listening methods for the messenger

        //listens to the scenario time update
        private void handleServerTimeUpdate(int currentServertime)
        {
            viewModelCurrentTime = currentServertime;

            plots = model.mainScenario.getNow(viewModelCurrentTime);
        }

        //listens to the scenario update
        private void handleScenarioUpdate(Scenario obj)
        {
            //create a temporary list to work on
            plots = obj.getNow(viewModelCurrentTime);
        }

        #endregion
    }
}
