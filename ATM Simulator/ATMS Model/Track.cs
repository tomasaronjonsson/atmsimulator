using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    [Serializable]
    public class Track
    {
        //to store the trackid 
        public int trackID;
        //to store the list of plots
        public List<Plot> plots;

        //to store the calsign
        public string callsign;


        //public location
       // public GeoPoint location;
        
        public Track()
        {
            plots = new List<Plot>();
            trackID = 0;
            callsign = "n/a";
        }

        public override string ToString()
        {
            return "Track ID: " + trackID;
        }
        /*
         * todo review 
         * we have to implement our own equals method to determin be sure we are talkinga lways about the same track
         * 
         * sprint 6
         * */
        public override bool Equals(object obj)
        {
            if (obj == null) 
                return false;

            Track objAsTrack = obj as Track;

            if (objAsTrack == null) 
                return false;

            if (objAsTrack.trackID == this.trackID) 
                return true;

            return false;
        }

        /**
         * todo review
         * 
         * to edit the track basically copy the information between the tracks when we pass it another track
         * sprint 6
         * */
        
        public void edit(Track t)
        {
            //todo implement nothing to be able to CHANGe we don't store anything interesting 

        }


    }
}
