﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ATMS_Model;

namespace ATMS_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class MainSimulation : IServerInterface
    {
        public Scenario s;

        //empty test constructor to create a new scenario
        public MainSimulation() { }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        

        //respond to poke method
        public string returnPoke()
        {
            return "Ouch";
        }


        public void createFirstScenario(Scenario s)
        {
            this.s = s;
        }
    }
}
