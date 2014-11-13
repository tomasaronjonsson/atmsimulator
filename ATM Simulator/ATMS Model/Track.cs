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
        //This is the main identifier of a track 
        public int trackID;
        //This list holds all the plots that form this track
        public List<Plot> plots;
        //The callSign identifies a flight on the map
        public string callSign;
        

        public Track()
        {
            //Initialize the list of Plots
            plots = new List<Plot>();
            //Upon initialization, default values are being assigned to the trackID and the callSign
            trackID = 0;
            callSign = "N/A";
        }

        //The edit method updates the changes made to a Track using a Track type parameter
        public void edit(Track t)
        {
            //Check the argument
            if (t.callSign != null)
                //Update the callSign using the Track argument
                this.callSign = t.callSign;
        }

        //A custom made Equals method to handle the List operations
        public override bool Equals(object obj)
        {
            //Return false if there is a missing parameter
            if (obj == null) 
                return false;

            //Cast the object to a Track object
            Track objAsTrack = obj as Track;

            //Return false of the casting is not successful
            if (objAsTrack == null) 
                return false;

            //If the casting is successful,
            //check the trackID to make sure the plots are the same.
            if (objAsTrack.trackID == this.trackID) 
                return true;
            return false;
        }

        //For prototype purposes, a custom ToString method 
        //prints out the relevant details of the current Track
        public override string ToString()
        {
            return "Track ID: " + trackID;
        }
    }
}
