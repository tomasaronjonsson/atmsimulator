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

        private PlotModel _plotModel;
        public PlotModel plotModel
        {
            get { return _plotModel; }
            set
            {
                _plotModel = value;
                OnPropertyChanged("plotModel");
            }
        }

        private string _serviceBox;
        public string serviceBox
        {
            get { return _serviceBox; }
            set
            {
                _serviceBox = value;
                OnPropertyChanged("serviceBox");
            }
        }
        public string resultBox { get; set; }
        public string timestampBox { get; set; }


        Model model;

        public ViewModel()
        {
            model = new Model(this);
            resultBox = "Click above to poke the server";

            plotModel = new PlotModel(new Plot("test"));

            timestampBox = "Set the first timestamp";
            TestWCF = new PokeServerCommand(this);
        }
        public ICommand TestWCF
        {
            get;
            private set;
        }

        internal void Poke()
        {
            model.poke();
        }

        internal void Register()
        {
            model.register(9999);
        }
    

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
