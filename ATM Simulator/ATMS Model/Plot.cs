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
        public double x; //position 
        public double y; //position 
        public double z; //altitude
        public double speed;

        public bool takeOver;

        public Plot()
        {
            this.takeOver = false;
        }

        public override string ToString()
        {
            return String.Format("{0} - X: {1} y: {2} z: {3} ", timestamp, x, y, z);
        }
    }
}
