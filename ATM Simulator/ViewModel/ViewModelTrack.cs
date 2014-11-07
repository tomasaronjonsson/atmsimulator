﻿using System;
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
    public class ViewModelTrack : INotifyPropertyChanged
    {


       


        /* todo review alex
        * 
        * make a consrtuctor that can take a track object in and initlize everything from that track
        * */
        public ViewModelTrack(Track track)
        {

            //copy the plots to a bindinglist
            plots = new BindingList<Plot>(track.plots);

            this.trackID = track.trackID;
            this.callsign = track.callsign;

            //default values for the row and the column
            this.column = 2;
            this.row = 0;

            UpdateTick();
        }
        /* todo review alex
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
        * todo review alex
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


        /*
         * todo review alex
         * 
         * we want a property storing the current plot
         */
        private Plot _currentPlot;
        public Plot currentPlot
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
        /*
         * todo review alex
         * 
         * we want a property storing the current location we can later implement geopoint into our plots and then we don't need this
         */

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


        /*
         * todo review alex
         * 
         * we want a property storing our trackid
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
                    RaisePropertyChanged("trackID");
                }
            }
        }

       

        /*
         * todo review Alex
         * 
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
                    RaisePropertyChanged("currentTime");
                }
            }
        }
        /*
         * todo review alex
         * 
         * we want a property storing the callsign
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
        * todo review alex
        * 
        * store our plots has a property and a binding list for binding purposes
        */
        private BindingList<Plot> _plots;
        public BindingList<Plot> plots
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
        

        /*
         * todo review alex
         * 
         * Everytime the currentime is updated this method is called
         * 
         */
        public void UpdateTick()
        {
            //find the plot for the timeframe
            var tempCurrentPlot = plots.First(x => x.time == currentTime);
            //check if we found anything
            if (tempCurrentPlot != null)
            {

                //update the current plot
                currentPlot = tempCurrentPlot;

                //update the current location
                currentLocation = new GeoPoint(currentPlot.latitude, currentPlot.longitude);
                //update the current altitude
                altitude = currentPlot.altitude;
            }
        }


        /*
         * todo review alex
         * 
         * return a track objet we can use to send to the server if we need have edited something or want to remove this track from the system
         */
        public Track toTrack()
        {
            Track tempTrack = new Track();

            tempTrack.callsign = callsign;
            tempTrack.trackID = trackID;
            return tempTrack;
        }


      
        /*
         * todo review alex
         * 
         * override the equals method for the remove command and etc.
         */
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            //try to convert the obj to viewmodeltrack or track to see if it's either one of those obj
            ViewModelTrack objasViewModelTrack = obj as ViewModelTrack;            
            Track objasTrack = obj as Track;


            //check if the convertion to ViewModeltrack was successfull and if so check if it's the same track id
            if (objasViewModelTrack != null)
                if (objasViewModelTrack.trackID == this.trackID)
                    return true;

            //same as above except now checking if it's track
            if (objasTrack != null)
                if (objasTrack.trackID == this.trackID)
                    return true;
            //all checks have failed not the same
            return false;
        }

        /*
         * todo alex review 
         * 
         * to store the current altitude, should be currentAltitude?
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


        /*
         *  todo review alex
         * 
         * we accept and track object we want to use to edit the information in this object

         */
        public void edit(Track t)
        {
            //check if the callsign on the foreign object is set
            if (t.callsign != null)
                this.callsign = t.callsign;
        }

        /*
         * todo review alex 
         * 
         *  overridiing the default tostring method
         */
        public override string ToString()
        {
            string tempString = "Trackid: " + trackID +
                   "\nCallsign: " + callsign;

            if (currentPlot != null)
            {

                tempString += "\nCourse: " + currentPlot.course +
                    "\nAltitude: " + currentPlot.altitude;
            }
            tempString += "\nNumber of plots: " + plots.Count;
            return tempString;
        }

        /*
        * todo review alex 
        * 
        *  implementing the intofiyproerty change interface
        */
        public event PropertyChangedEventHandler PropertyChanged;
        /*
        * todo review alex 
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
