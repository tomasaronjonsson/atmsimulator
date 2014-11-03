﻿using System;
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

        public Track()
        {
            plots = new List<Plot>();
        }

        public override string ToString()
        {
            return "Track ID: " + trackID;
        }
    }
}
