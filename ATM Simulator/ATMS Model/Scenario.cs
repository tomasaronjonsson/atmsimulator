using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    public class Scenario
    {
        //to store the list of tracks witihin the scenario
        public List<Track> tracks { get; set; }

        public Scenario()
        {
            tracks = new List<Track>();
        }
    }
}
