using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ATMS_Model
{
    [Serializable]
    public class Scenario
    {
        //to store the list of tracks witihin the scenario
        public List<Track> tracks { get; set; }


        public Scenario()
        {
            tracks = new List<Track>();
        }

    

        //returns a list of plots at the current time in seconds
        public List<Plot> getNow(int seconds)
        {

            var gettingAllPlots = tracks.SelectMany(x => x.plots.Where(p => (p.time == seconds)));

            return gettingAllPlots.ToList();
        }

    }
}
