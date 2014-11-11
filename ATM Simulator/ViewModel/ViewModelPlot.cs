using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Map;
using System.ComponentModel;
using ATMS_Model;

namespace ViewModel
{
    public class ViewModelPlot : INotifyPropertyChanged
    {

        /*
         * 
         * Store the location of the plot 
         */
        
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
        /*
         * store the trackID the plot belongs to
         * 
         */
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

        /*
        * store the time
        * 
        */
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
        /*
        * store the speed at this plot
        * 
        */
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
        /*
        * store the course at this plot
        * 
        */
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
        /*
       * store the altitude at this plot
       * 
       */
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
        

        

        public ViewModelPlot(Plot p)
        {
            //take the foreign information
            edit(p);


        }



        public void edit(Plot p)
        {
            this.location = new GeoPoint(p.latitude, p.longitude);

            this.trackID = p.trackID;

            this.time = p.time;

            this.speed = p.speed;

            this.course = p.course;

            this.altitude = p.altitude;
        }
        /*
         * to be able to return a plot
         */
        public Plot toPlot()
        {
            Plot p = new Plot();

            p.altitude = this.altitude;
            p.course = this.course;
            p.latitude = this.location.Latitude;
            p.longitude = this.location.Longitude;
            p.time = this.time;
            p.trackID = this.trackID;

            return p;
        }



        /*
        * 
        * override the equals method for the remove command and etc.
        */
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            //try to convert the obj to ViewModelPlot or Plot to see if it's either one of those obj
            ViewModelPlot objasViewModelPlot = obj as ViewModelPlot;
            Plot objasPlot = obj as Plot;


            //check if the convertion to objasViewModelPlot was successfull and if so check if it's the same track id and time
            if (objasViewModelPlot != null)
                if ((objasViewModelPlot.trackID == this.trackID) && (objasViewModelPlot.time == this.time))
                    return true;

            //same as above except now checking if it's plot and if the trackid and time is the same that defines it is the same plot
            if (objasPlot != null)
                if ((objasPlot.trackID == this.trackID) && (objasPlot.time == this.time))
                    return true;
            //all checks have failed not the same
            return false;
        }

        /*
       * 
       *  implementing the intofiyproerty change interface
       */
        public event PropertyChangedEventHandler PropertyChanged;
        /*
        * 
        *  Create the RaisePropertyChanged method to raise the event 
        */
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
