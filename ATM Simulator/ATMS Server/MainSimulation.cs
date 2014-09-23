using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace ATMS_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MainSimulation : IServerInterface
    {


        public MainSimulation()
        {


        }



        //respond to poke method
        public string ReturnPoke()
        {
            hiClient();
            return "Ouch";
        }

        //to test the callback
        public void hiClient()
        {
            Proxy.updateClient("Hi!");
        }


        public IClientCallbackInterface Proxy
        {
            get
            {

                return OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();
            }
        }
    }
}
