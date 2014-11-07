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

namespace Model
{
    public class ViewModelTrack : Track, INotifyPropertyChanged
    {
        


        //make a consrtuctor that can take the base class inn to change it to the derivedd version
        public ViewModelTrack(Track track) {
            base.plots = track.plots;
            trackid = track.trackID;
            callsign = track.callsign;



            UpdateTick();
        }
        /*
         * 
         * for later use, this can be used by the gui to determin where the user want his information for the track to appear in a 3x3 matrix
         * */
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
        /*
        * 
        * for later use, this can be used by the gui to determin where the user want his information for the track to appear in a 3x3 matrix
        * */
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
        
        

        //we want a property storing the current location
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

        private double _currentCourse;
        public double currentCourse
        {
            get { return _currentCourse; }
            set
            {
                if (value != _currentCourse)
                {
                    _currentCourse = value;
                    RaisePropertyChanged("currentCourse");
                }
            }
        }
        
        private MapDot _mapObject;
        public MapDot mapObject
        {
            get { return _mapObject; }
            set
            {
                if (value != _mapObject)
                {
                    _mapObject = value;
                    RaisePropertyChanged("mapObject");
                }
            }
        }
        
        /*
         * we want the scenario to update the current time on the track and the track will then update its location 
         * and update the currentlocation property, iof the current property is the saem as before, example . the track has no location, the map doesn't 
         * need to update that track, less load
        */
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
                }
            }
        }

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

        private int _trackid;
        public int trackid
        {
            get { return _trackid; }
            set
            {
                if (value != _trackid)
                {
                    _trackid = value;
                    RaisePropertyChanged("trackid");
                }
            }
        }
        
        


        /*
         * Everytime the currentime is updated this method is called
         * 
         * 
         * */
        public void UpdateTick()
        {
          //find the plot for the timeframe
            var currentPlot = plots.First(x => x.time == currentTime);
            //check if we found anything
            if (currentPlot != null)
            {

                //update the location 

                //let's create a new geo point for the new location
                GeoPoint geo = new GeoPoint();
                geo.Latitude = currentPlot.latitude;
                geo.Longitude = currentPlot.longitude;
                //lets safe the new location this should notifiy then the gui
                currentLocation = geo;

                currentCourse = currentPlot.course;
            }
            
            


        }
        


        // implementing the intofiyproerty change interface
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public override string ToString()
        {
            return "Trackid: " + trackid +
                   "callsign: " + callsign +
                    "\nCourse: " + currentCourse +
                    "\nNumber of plots: " + plots.Count;
        }
    }
}
