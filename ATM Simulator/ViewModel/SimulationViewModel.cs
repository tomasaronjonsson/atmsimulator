﻿using ATMS_Model;
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



            _plots = new List<Plot>();

        }
        private void handleScenarioUpdate(Scenario obj)
        {
            List<Plot> temp = new List<Plot>();

            //var temp = obj.tracks.Select(x => x.plots.Select(a => a));
            //plots = (List<Plot>)temp;


            foreach (Track t in obj.tracks)
            {
                foreach(Plot p in t.plots)
                {
                    temp.Add(p);
                }
            }

            plots = temp;

           // RaisePropertyChanged("plots");



        }
    }
}
