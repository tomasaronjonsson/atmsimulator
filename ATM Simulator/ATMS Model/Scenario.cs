using System;
using System.Collections.Generic;
using System.Linq;

namespace ATMS_Model
{
    [Serializable]
    public class Scenario
    {
        //This list holds the tracks that belong to this Scenario
        public List<Track> tracks { get; set; }


        public Scenario()
        {
            //Initialize the list of Tracks
            tracks = new List<Track>();
        }
    }
}
