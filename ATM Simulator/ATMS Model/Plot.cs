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

        public DateTime timestamp;
        public double latitude; 

        public double longitude; 

        public double altitude; 

        public double speed;

        public bool takeOver;

        public Plot()
        {
            this.takeOver = false;
        }

        public override string ToString()
        {
            return String.Format("{0} - latitude: {1} longitude: {2} altitude: {3} ", timestamp, latitude, longitude, altitude);
        }
    }
}
