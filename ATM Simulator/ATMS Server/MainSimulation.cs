using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using ATMS_Model;

namespace ATMS_Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MainSimulation : IServerInterface
    {
        private static Dictionary<int, IClientCallbackInterface> clients = new Dictionary<int, IClientCallbackInterface>();

        public List<Scenario> simulation;

        public MainSimulation()
        {
        }

        //respond to poke method
        public Plot ReturnPoke()
        {
            return new Plot("OK");
        }

        //to test the callback
        public void hiClient()
        {
            Thread.Sleep(5000);

            foreach (KeyValuePair<int, IClientCallbackInterface> entry in clients)
                entry.Value.updateClient("OK");
        }

        public int RegisterClient(int id)
        {
            ThreadPool.QueueUserWorkItem(a => hiClient());
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

        //From here

        public void notifyClients()
        {
            Thread.Sleep(2000);
            foreach (KeyValuePair<int, IClientCallbackInterface> entry in clients)
                entry.Value.updateClient(simulation.Count().ToString());
        }

        public void createSimulation(string timestamp)
        {
            Plot p = new Plot(timestamp);
            Track t = new Track(p);
            Scenario s = new Scenario(t);

            try
            {
                simulation = new List<Scenario>();
                simulation.Add(s);
            }
            catch (Exception)
            {
                throw;
            };
            ThreadPool.QueueUserWorkItem(a => notifyClients());
        }
    }
}
