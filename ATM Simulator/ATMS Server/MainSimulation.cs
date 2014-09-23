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

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MainSimulation : IServerInterface
    {
        private static Dictionary<int, IClientCallbackInterface> clients = new Dictionary<int,IClientCallbackInterface>();
        private static object locker = new object();


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


        public void RegisterClient(int id)
        {
            if (id > 999)
            {
                try
                {
                    IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();
                    lock (locker)
                    {
                        //remove the old client
                        if (clients.Keys.Contains(id))
                            clients.Remove(id);
                        clients.Add(id, callback);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
