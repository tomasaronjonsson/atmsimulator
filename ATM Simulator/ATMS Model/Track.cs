using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    public class Track
    {
        //to store the trackid 
        public int trackID {get; set; }
        //to store the list of plots
        public List<Plot> plots { get; set; }

        public Track()
        {
            plots = new List<Plot>();
        }
    }
}
