using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    [Serializable]
    public class Plot
    {
        //The time is the main identifier of a Plot
        public int time;

        //This holds the ID of the track to which the Plot belongs to
        public int trackID;

        /*
         * The position of a plot is determined using 
         * the following 3 variables
         * -latitude
         * -longitude
         * -altitude
         * */
        public double latitude;
        public double longitude;
        public double altitude;

        //The directional orientation of the flight
        public double course;

        //The speed of the flight
        public double speed;


        public Plot()
        {
        }


        //The edit method updates the changes made to a plot using a Plot type parameter
        public void edit(Plot p)
        {
            //Check the argument
            if (p != null)
            {
                //Each variable of the incoming argument overwrites the current plot variables
                this.latitude = p.latitude;
                this.longitude = p.longitude;
                this.speed = p.speed;
                this.course = p.course;
                this.altitude = p.altitude;
            }
        }

        //This equals method is used to compare plots when adding or removing from a list of plots
        public override bool Equals(object obj)
        {
            //Return false if there is a missing parameter
            if (obj == null)
                return false;

            //Cast the object to a Plot object
            Plot objAsPlot = obj as Plot;

            //Return false of the casting is not successful
            if (objAsPlot == null)
                return false;

            //If the casting is successful,
            //check the trackID & the time to make sure the plots are the same.
            if (objAsPlot != null)
                if ((objAsPlot.trackID == this.trackID) && (objAsPlot.time == this.time))
                    return true;
            return false;
        }

        //For prototype purposes, a custom ToString method 
        //prints out the relevant details of the current plot
        public override string ToString()
        {
            return String.Format("Plot time: {0} / LAT: {1} / LON: {2} / ALT: {3}.", time, latitude.ToString("#.00000000"), longitude.ToString("#.00000000"), altitude);
        }
    }
}
