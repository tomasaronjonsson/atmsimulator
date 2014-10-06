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
        //for testing
        public string test { get; set; }
        public DateTime timestamp { get; set; }
        public double x; //position 
        public double y; //position 
        public double z; //altitude
        public double speed;

        public bool takeOver; //needs to be reviewed

        public Plot()
        {
            this.takeOver = false;
        }


        //for testing
        public Plot(String data)
        {
            this.test = data;
            this.takeOver = false;
        }


        public string ToString()
        {
            return String.Format("{0} - X: {1} y: {2} z: {3} ", timestamp, x, y, z);
        }
    }
}
