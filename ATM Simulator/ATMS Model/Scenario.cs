using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    public class Scenario
    {
        public int scenarioID;
        public List<Track> tracks;

        public Scenario(int id)
        {
            scenarioID = id;
            tracks = new List<Track>();
        }

        //Test constructor to initialize a prototype scenario
        public Scenario(Track t1)
        {
            scenarioID = 001;
            tracks = new List<Track>();
            tracks.Add(t1);
        }
    }
}
