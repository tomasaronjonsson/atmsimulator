using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    class Plot
    {
        public DateTime timestamp;
        public double x; //position 
        public double y; //position 
        public double z; //altitude
        public double speed;

        public bool takeOver; //needs to be reviewed

        //a test constructor to initialize a plot with the minimum required arguments
        public Plot(DateTime timestamp, bool takeOver)
        {
            this.timestamp = timestamp;
            this.takeOver = false;
        }
    }
}
