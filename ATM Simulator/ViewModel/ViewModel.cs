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
        List<Plot> plots = new List<Plot>();

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
            Messenger.Default.Register<Scenario>(this, handleScenarioUpdate);
        }

        private void handleScenarioUpdate(Scenario obj)
        {
            plots.Clear();

            plots = obj.tracks
        }
    }
}
