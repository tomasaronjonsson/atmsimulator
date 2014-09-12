﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    class Scenario
    {
        public int scenarioID;
        public List<Track> tracks;

        //Test constructor to initialize a prototype scenario
        public Scenario(Track t1)
        {
            scenarioID = 001;
            tracks = new List<Track>();
            tracks.Add(t1);
        }
    }
}
