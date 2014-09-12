using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    public class Plot
    {
        public string timestamp; //made it string just for test purpsoe
        public double x; //position 
        public double y; //position 
        public double z; //altitude
        public double speed;

        public bool takeOver; //needs to be reviewed

        //a test constructor to initialize a plot with the minimum required arguments
        public Plot(string timestamp)
        {
            this.timestamp = timestamp.;
            this.takeOver = false;
        }
    }
}
