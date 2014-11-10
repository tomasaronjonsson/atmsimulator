using ATMS_Model;
using ATMS_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class CallbackHandler : Model.ServiceReference1.IServerInterfaceCallback
    {
        SimulationModel model;

        public CallbackHandler(SimulationModel model)
        {
            this.model = model;
        }

        public void notifyNewScenario(Scenario data)
        {
            model.mainScenario = data;
        }

        public void notifyTimeUpdate(int currentServerTime)
        {
            model.currentServerTime = currentServerTime;
            model.serverIsPlaying = true;
        }

        public void notifyNewTrack(Track t)
        {
            model.notifyNewTrack(t);
        }

        public void notifyRemoveTrack(Track t)
        {
            model.notifyRemoveTrack(t);
        }


        public void notifyEditedTrack(Track t)
        {
            model.notifyEditedTrack(t);
        }
    }
}
