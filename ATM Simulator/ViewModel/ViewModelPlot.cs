using ATMS_Model;
using DevExpress.Xpf.Map;
using System.ComponentModel;

namespace ViewModel
{
    /*
     * This is the ViewModel representation of a Plot
     * */
    public class ViewModelPlot : INotifyPropertyChanged
    {
        #region Properties

        // Store the longitude of the plot
        private double _longitude;
        public double longitude
        {
            get { return _longitude; }
            set
            {
                if (value != _longitude)
                {
                    _longitude = value;
                    _location.Longitude = _longitude;
                    RaisePropertyChanged("longitude");
                }
            }
        }

        // Store the latitude of the plot
        private double _latitude;
        public double latitude
        {
            get { return _latitude; }
            set
            {
                if (value != _latitude)
                {
                    _latitude = value;
                    _location.Latitude = _latitude;
                    RaisePropertyChanged("latitude");
                }
            }
        }

        // Store the location of the plot in a GeoPoint
        private GeoPoint _location;
        public GeoPoint location
        {
            get { return _location; }
            set
            {
                if (value != _location)
                {
                    _location = value;
                    RaisePropertyChanged("location");
                }
            }
        }

        //Store the altitude of the plot
        private double _altitude;
        public double altitude
        {
            get { return _altitude; }
            set
            {
                if (value != _altitude)
                {
                    _altitude = value;
                    RaisePropertyChanged("altitude");
                }
            }
        }

        // Store the trackID of the track that the current plot belongs to
        private int _trackID;
        public int trackID
        {
            get { return _trackID; }
            set
            {
                if (value != _trackID)
                {
                    _trackID = value;
                    RaisePropertyChanged("trackid");
                }
            }
        }

        //Store the time of the plot
        private int _time;
        public int time
        {
            get { return _time; }
            set
            {
                if (value != _time)
                {
                    _time = value;
                    RaisePropertyChanged("time");
                }
            }
        }

        //Store the speed of the flight at that plot
        private double _speed;
        public double speed
        {
            get { return _speed; }
            set
            {
                if (value != _speed)
                {
                    _speed = value;
                    RaisePropertyChanged("speed");
                }
            }
        }

        //Store the course of the plot
        private double _course;
        public double course
        {
            get { return _course; }
            set
            {
                if (value != _course)
                {
                    _course = value;
                    RaisePropertyChanged("course");
                }
            }
        }

        #endregion


        /*
         * Initialize the ViewModelPlot using a real Plot
         * */
        public ViewModelPlot(Plot p)
        {
            if (p != null)
            {
                edit(p);
            }
        }

        public ViewModelPlot() { }


        //Edit the current ViewModelPlot using the input Plot
        public void edit(Plot p)
        {
            //Validate the input
            if (p != null)
            {
                this.location = new GeoPoint(p.latitude, p.longitude);
                this.altitude = p.altitude;
                this.trackID = p.trackID;
                this.time = p.time;
                this.speed = p.speed;
                this.course = p.course;
            }
        }

        //Convert the ViewModelPlot to a Plot
        public Plot toPlot()
        {
            Plot p = new Plot();

            p.altitude = this.altitude;
            p.course = this.course;
            p.latitude = this.location.Latitude;
            p.longitude = this.location.Longitude;
            p.time = this.time;
            p.trackID = this.trackID;
            p.speed = this.speed;
            return p;
        }

        //A custom made Equals method to handle the List operations
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            //Convert the input into both a ViewModelPlot and a Plot
            ViewModelPlot objAsViewModelPlot = obj as ViewModelPlot;
            Plot objAsPlot = obj as Plot;


            //Validate objasViewModelPlot and then check the trackID and thetime in order to identify the plot
            if (objAsViewModelPlot != null)
                if ((objAsViewModelPlot.trackID == this.trackID) && (objAsViewModelPlot.time == this.time))
                    return true;

            //Validate objAsPlot and then check the trackID and thetime in order to identify the plot
            if (objAsPlot != null)
                if ((objAsPlot.trackID == this.trackID) && (objAsPlot.time == this.time))
                    return true;

            //If both checks fail - return false
            return false;
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
