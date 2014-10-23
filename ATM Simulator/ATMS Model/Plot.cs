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
            return String.Format("{0} - LAT: {1} LON: {2} ALT: {3} ", timestamp.ToString("hh:mm:ss"), latitude.ToString("#.00000000"), longitude.ToString("#.00000000"), altitude);
        }
    }
}
