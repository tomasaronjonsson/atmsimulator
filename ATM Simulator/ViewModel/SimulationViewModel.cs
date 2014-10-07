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

        //this is the create scenario command that calls the create scenario method from the model
        private RelayCommand _CreateScenario;
        public RelayCommand CreateScenario
        {
            get
            {
                if (_CreateScenario == null)
                {
                    _CreateScenario = new RelayCommand(
                       () =>
                       {
                           model.createScenario();
                       },
                       () =>
                       {
                           return model.isServerAvailable;
                       });
                }
                return _CreateScenario;
            }
        }

        public SimulationViewModel()
        {
            model = new SimulationModel();

            //This listens to the brodcasted messages sent by the Messenger
            Messenger.Default.Register<Scenario>(this, handleScenarioUpdate);

            //initialize the plots list
            _plots = new List<Plot>();
        }

        private void handleScenarioUpdate(Scenario obj)
        {
            if (obj != null)
            {
                //create a temporary list to work on
                List<Plot> temp = new List<Plot>();

                foreach (Track t in obj.tracks)
                {
                    foreach (Plot p in t.plots)
                    {
                        temp.Add(p);
                    }
                }
                plots = temp;
            }
        }
    }
}
