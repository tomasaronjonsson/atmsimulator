﻿using ATMS_Model;
using ATMS_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class CallbackHandler : Model.ATMS_Service.IServerInterfaceCallback
    {

        SimulationModel model;

        public CallbackHandler(SimulationModel model)
        {
            this.model = model;    
        }


        public void updateClient(string data)
        {
            //throw new NotImplementedException();
            
        }

        public void notifyNewScenario(Scenario data)
        {
            model.mainScenario = data;
        }

    }
}
