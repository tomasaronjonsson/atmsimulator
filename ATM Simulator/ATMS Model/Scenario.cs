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



        //to imlpement basetime datetimestamp + seconds > 
        private DateTime basetime;



        public Scenario()
        {
            tracks = new List<Track>();
            basetime = DateTime.MinValue;

        }


        //returns a list of plots at the current time in seconds
        public List<Plot> getNow(int seconds)
        {

            if (basetime == DateTime.MinValue)
            {
                //if basetime is not set, find the basetime for the scenario, it only needs to do this once
                // should this be strong typed?
                try
                {
                    var query = tracks.SelectMany(x => x.plots.Select(l => l.timestamp)).Min();
                    basetime = (DateTime)query;
                }
                catch (InvalidOperationException)
                {
                }
            }

            var gettingAllPlots = tracks.SelectMany(x =>
                x.plots.Where(p => (p.timestamp < basetime.AddSeconds(seconds + BuisnessLogicValues.radarInterval))
                                && (p.timestamp >= basetime.AddSeconds(seconds))
            ));

            return gettingAllPlots.ToList();
        }
    }
}
