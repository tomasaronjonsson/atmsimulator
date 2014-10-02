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
        private static Dictionary<int, IClientCallbackInterface> clients;

        public List<Scenario> simulation;
        int scenarioIDSigner;

        public MainSimulation()
        {
            scenarioIDSigner = 1;
            clients = new Dictionary<int, IClientCallbackInterface>();
            simulation = new List<Scenario>();
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
                entry.Value.updateClient(String.Format("OK {0}", clients.Count().ToString()));
        }

        public int RegisterClient(int id)
        {

            if (id > 999)
            {
                try
                {
                    IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();
                    clients.Add(id, callback);
                    ThreadPool.QueueUserWorkItem(a => hiClient());
                }
                catch (Exception)
                {
                }
                return id;
            }
            return -1;
        }

        //From here

        public void notifyClients()
        {
            Thread.Sleep(5000);

            foreach (KeyValuePair<int, IClientCallbackInterface> entry in clients)
                entry.Value.notifyNewScenario(String.Format("We currently have {0} new scenarios.", simulation.Count().ToString()));
        }

        public void createSimulation()
        {
            Scenario s = new Scenario(scenarioIDSigner);
            try
            {
                simulation.Add(s);
                ThreadPool.QueueUserWorkItem(a => notifyClients());
                scenarioIDSigner++; //increment the ID
            }
            catch (Exception)
            {
                throw;
            };
        }
    }
}
