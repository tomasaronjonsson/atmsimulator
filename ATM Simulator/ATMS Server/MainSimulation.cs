using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;


namespace ATMS_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MainSimulation : IServerInterface
    {
        private static Dictionary<int, IClientCallbackInterface> clients = new Dictionary<int,IClientCallbackInterface>();
       


        public MainSimulation()
        {
            

        }



        //respond to poke method
        public string ReturnPoke()
        {
            ThreadPool.QueueUserWorkItem(a => hiAfter5sec());
            return "Ouch";
        }
        public void hiAfter5sec()
        {
            Thread.Sleep(5000);
            hiClient();

        }
        //to test the callback
        public void hiClient()
        {
            foreach (KeyValuePair<int,IClientCallbackInterface> entry in clients)
                entry.Value.updateClient("hi!");

        }

        public int RegisterClient(int id)
        {
            
            if (id > 999)
            {
                try
                {
                    IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();
                    clients.Add(id, callback);


                    
                }
                catch (Exception ex)
                {
                }

                return id;
            }
            return -1;

        }
    }
}
