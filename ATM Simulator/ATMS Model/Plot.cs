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
        //decided in sprint 6 to remove the datetime and change it to an int
        public int time;

        public double latitude;

        public double longitude;

        public double altitude;

        public double course;

        public double speed;

        public int trackID;

        public Plot()
        {
        }
        /*
         * todo review this
         * */
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Plot objAsPlot = obj as Plot;

            if (objAsPlot == null)
                return false;

            if (objAsPlot != null)
                if ((objAsPlot.trackID == this.trackID) && (objAsPlot.time == this.time))
                    return true;

            return false;
        }

        public void edit(Plot p)
        {
            //if the callsign is set on the remote track, we take it
            if (p != null)
            {
                this.latitude = p.latitude;
                this.longitude = p.longitude;
                this.speed = p.speed;
                this.course = p.course;
                this.altitude = p.altitude;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}     LAT: {1}     LON: {2}     ALT: {3}", time, latitude.ToString("#.00000000"), longitude.ToString("#.00000000"), altitude);
        }

        public object serviceNumber { get; set; }
    }
}
