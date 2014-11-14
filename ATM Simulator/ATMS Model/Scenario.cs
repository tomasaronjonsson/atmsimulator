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


        //This method returns a list of Lots at the given time
        public List<Plot> getNow(int time)
        {
            //Lambda expression to select all the plots from each track where the time is equal to the input time
            var gettingAllPlots = tracks.SelectMany(x => x.plots.Where(p => (p.time == time)));

            //Convert the above var to a List and return it
            return gettingAllPlots.ToList();
        }
    }
}
