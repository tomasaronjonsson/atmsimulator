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

        public override string ToString()
        {
            return String.Format("{0}     LAT: {1}     LON: {2}     ALT: {3}", time, latitude.ToString("#.00000000"), longitude.ToString("#.00000000"), altitude);
        }

        public object serviceNumber { get; set; }
    }
}
