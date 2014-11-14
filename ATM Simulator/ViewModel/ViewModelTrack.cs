using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMS_Model;
using DevExpress.Xpf.Map;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using System.Windows;
using ViewModel;

namespace Model
{
    /*
     * This is the ViewModel representation of a Track
     * */
    public class ViewModelTrack : INotifyPropertyChanged
    {
        #region Properties

        //Stores the current plot
        private ViewModelPlot _currentPlot;
        public ViewModelPlot currentPlot
        {
            get { return _currentPlot; }
            set
            {
                if (value != _currentPlot)
                {
                    _currentPlot = value;
                    RaisePropertyChanged("currentPlot");
                }
            }
        }

        //Stores the current location
        private GeoPoint _currentLocation;
        public GeoPoint currentLocation
        {
            get { return _currentLocation; }
            set
            {
                if (value != _currentLocation)
                {
                    _currentLocation = value;
                    RaisePropertyChanged("currentLocation");
                }
            }
        }

        //Stores the track id
        private int _trackID;
        public int trackID
        {
            get { return _trackID; }
            set
            {
                if (value != _trackID)
                {
                    _trackID = value;
                    RaisePropertyChanged("trackID");
                }
            }
        }

        /*
         * Stores the current time
         * 
         * Every time the current time is updated call the UpdateTick method that updates the location and the current plot
         * */
        private int _currentTime;
        public int currentTime
        {
            get { return _currentTime; }
            set
            {
                if (value != _currentTime)
                {
                    _currentTime = value;
                    UpdateTick();
                    RaisePropertyChanged("currentTime");
                }
            }
        }

        /*
         * Stores the callsign
         */
        private string _callsign;
        public string callsign
        {
            get { return _callsign; }
            set
            {
                if (value != _callsign)
                {
                    _callsign = value;
                    RaisePropertyChanged("callsign");
                }
            }
        }

        /*
         *  Stores the current altitude
         * */
        private double _currentAltitude;
        public double currentAltitude
        {
            get { return _currentAltitude; }
            set
            {
                if (value != _currentAltitude)
                {
                    _currentAltitude = value;
                    RaisePropertyChanged("altitude");
                }
            }
        }

        /*
         * Stores the list of ViewModelPlots
         * */
        private BindingList<ViewModelPlot> _plots;
        public BindingList<ViewModelPlot> plots
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

        //Graphical positioning of the track information
        private int _column;
        public int column
        {
            get { return _column; }
            set
            {
                if (value != _column)
                {
                    _column = value;
                    RaisePropertyChanged("column");
                }
            }
        }

        private int _row;
        public int row
        {
            get { return _row; }
            set
            {
                if (value != _row)
                {
                    _row = value;
                    RaisePropertyChanged("row");
                }
            }
        }

        #endregion


        public ViewModelTrack(Track t)
        {
            //Initialize the list of ViewModelPlots
            plots = new BindingList<ViewModelPlot>();

            //Populate the list of plots
            t.plots.ForEach(delegate(Plot p)
                {
                    plots.Add(new ViewModelPlot(p));
                }
            );

            //Handle the input
            edit(t);

            //Default values for the track information positioning
            this.column = 2;
            this.row = 0;

            //Update the current plot
            UpdateTick();
        }


        /*
         * This method updates the track information according to the current time
         * */
        public void UpdateTick()
        {
            //Find the current plot
            var tempCurrentPlot = plots.FirstOrDefault(x => x.time == currentTime);

            //Validate the current plot
            if (tempCurrentPlot != null)
            {
                //Update the current plot
                currentPlot = tempCurrentPlot;

                //Update the current altitude
                currentAltitude = currentPlot.altitude;
            }
        }

        // Convert the current ViewModelTrack to a Track
        public Track toTrack()
        {
            Track tempTrack = new Track();

            tempTrack.callSign = callsign;
            tempTrack.trackID = trackID;
            return tempTrack;
        }

        //A custom made Equals method to handle the List operations
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            //Convert the input into both a ViewModeltrack and a Track
            ViewModelTrack objasViewModelTrack = obj as ViewModelTrack;
            Track objasTrack = obj as Track;


            //Validate the ViewModelTrack
            if (objasViewModelTrack != null)
                if (objasViewModelTrack.trackID == this.trackID)
                    return true;

            //Validate the Track
            if (objasTrack != null)
                if (objasTrack.trackID == this.trackID)
                    return true;

            //If both checks fail - return false
            return false;
        }

        //Edit the current ViewModelPlot using the input Plot
        public void edit(Track t)
        {
            // Validate the input
            if (t.callSign != null)
            {
                this.trackID = t.trackID;
                this.callsign = t.callSign;
            }
        }

        /*
         * A custom made ToString method for the ASTERIX module
         * */
        public override string ToString()
        {
            string tempString = "Trackid: " + trackID + "\nCallsign: " + callsign;

            //Validate the current plot
            if (currentPlot != null)
            {
                tempString += "\nCourse: " + currentPlot.course + "\nAltitude: " + currentPlot.altitude;
            }

            tempString += "\nNumber of plots: " + plots.Count;
            return tempString;
        }


        /*
         * Here is the implementation of the INotifyPropertyChanged
         * */
        #region PropertyChanged implementation

        //PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        //Raise property changed
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
