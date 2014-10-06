using ATMS_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ATMS_Client.Commands;
using System.ComponentModel;
using ATMS_Model;

namespace ATMS_Client.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        Model model;



        #region properties

        PlotModel _plotModel;
        public PlotModel plotModel
        {
            get { return _plotModel; }
            set
            {
                _plotModel = value;
                OnPropertyChanged("plotModel");
            }
        }

        string _callbackBox;
        public string callbackBox
        {
            get { return _callbackBox; }
            set
            {
                _callbackBox = value;
                OnPropertyChanged("callbackBox");
            }
        }

        string _newScenarioString;
        public string newScenarioString
        {
            get { return _newScenarioString; }
            set
            {
                _newScenarioString = value;
                OnPropertyChanged("newScenarioString");
            }
        }

        private bool _serverAvailable = false;
        public bool serverAvailable
        {
            get { return _serverAvailable; }
            set
            {
                _serverAvailable = value;
                OnPropertyChanged("serverAvailable");
            }
        }
        #endregion

        public ViewModel()
        {
            model = new Model(this);
            plotModel = new PlotModel(new Plot("test"));
            TestWCF = new PokeServerCommand(this);
            CreateScenario = new CreateScenarioCommand(this);
        }

        #region ICommands
        public ICommand TestWCF
        {
            get;
            private set;
        }

        public ICommand CreateScenario
        {
            get;
            private set;
        }
        #endregion

        #region methods
        internal void Poke()
        {
            model.poke();
        }

        internal void Register()
        {
            model.register();

        }

        internal void newScenario()
        {
            model.newScenario();
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
