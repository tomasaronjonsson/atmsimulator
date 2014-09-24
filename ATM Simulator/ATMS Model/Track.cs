using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    public class Track
    {
        public int trackID;
        public List<Plot> plots;

        //Test constructor to initialize a prototype track
        public Track(Plot p1)
        {
            trackID = 001;
            plots = new List<Plot>();
            plots.Add(p1);
        }
    }
}
