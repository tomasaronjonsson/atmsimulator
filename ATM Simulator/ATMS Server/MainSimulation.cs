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


        private int availableClientID = 1000;

        //declaring the scenario we will have has the "main" scenario, where changes are saved etc.
        private Scenario mainScenario;



        //declaring a dictionary to store the layered scenarios , the 
        private Dictionary<int, Scenario> layeredScenarios;


        //declaring an integer to automatically increment when we add layered scenarios
        private int scenarioIDSigner;

        public MainSimulation()
        {
            scenarioIDSigner = 1;
            clients = new Dictionary<int, IClientCallbackInterface>();
            layeredScenarios = new Dictionary<int, Scenario>();
        }


        #region test
        //respond to poke method
        public Plot ReturnPoke()
        {
            return new Plot("Ok");
        }

        //to test the callback
        public void hiClient()
        {
            Thread.Sleep(5000);

            foreach (KeyValuePair<int, IClientCallbackInterface> entry in clients)
                entry.Value.updateClient(String.Format("OK {0}", clients.Count().ToString()));
        }
        #endregion test

        public int RegisterClient(int id)
        {

            if (id < 1000)
            {
                while (clients.ContainsKey(availableClientID))
                {
                    availableClientID++;
                }
                id = availableClientID;
            }

            try
            {
                IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();
                clients.Add(id, callback);
                //for testing / debugging purposes
                ThreadPool.QueueUserWorkItem(a => hiClient());
            }
            catch (Exception) { }

            return id;
        }

        public void notifyClients()
        {
            Thread.Sleep(5000);

            foreach (KeyValuePair<int, IClientCallbackInterface> entry in clients)
                entry.Value.notifyNewScenario(mainScenario);
        }

        public void createScenario()
        {
            mainScenario = new Scenario();
            try
            {
                ThreadPool.QueueUserWorkItem(a => notifyClients());
            }
            catch (Exception)
            {
                throw;
            };
            //for debugging purposes
            populateScenario(mainScenario);
        }

        //test method for populating scenarios with test data
        private void populateScenario(Scenario sc)
        {

            //here we are going to add 3 tracks, with 3 plots each for testing later

            #region track1
            //creating the first track for out test scenario
            Track t1 = new Track();

            //first plot within the first track
            Plot t1_1 = new Plot();
            t1_1.timestamp = DateTime.Now;
            t1_1.speed = 500;
            t1_1.x = 20;
            t1_1.y = 30;
            t1_1.z = 40;
            //add the plot to the track
            t1.plots.Add(t1_1);

            //second plot within the first track after 4 seonds
            Plot t1_2 = new Plot();
            t1_2.timestamp = DateTime.Now.AddSeconds(4);
            t1_2.speed = 500;
            t1_2.x = 25;
            t1_2.y = 35;
            t1_2.z = 35;
            //add the plot to the track
            t1.plots.Add(t1_2);


            //third plot within the first track after 8 seonds
            Plot t1_3 = new Plot();
            t1_3.timestamp = DateTime.Now.AddSeconds(8);
            t1_3.speed = 500;
            t1_3.x = 30;
            t1_3.y = 40;
            t1_3.z = 30;
            //add the plot to the track
            t1.plots.Add(t1_3);



            #endregion track1

            //addting track 1 the scenario
            sc.tracks.Add(t1);


            #region track2
            //creating the second track for out test scenario
            Track t2 = new Track();

            //first plot within the second track
            Plot t2_1 = new Plot();
            t2_1.timestamp = DateTime.Now;
            t2_1.speed = 450;
            t2_1.x = 120;
            t2_1.y = 130;
            t2_1.z = 140;
            //add the plot to the track
            t2.plots.Add(t1_1);

            //second plot within the second track after 4 seonds
            Plot t2_2 = new Plot();
            t2_2.timestamp = DateTime.Now.AddSeconds(4);
            t2_2.speed = 500;
            t2_2.x = 25;
            t2_2.y = 35;
            t2_2.z = 35;


            //third plot within the second track after 8 seonds
            Plot t2_3 = new Plot();
            t2_3.timestamp = DateTime.Now.AddSeconds(8);
            t2_3.speed = 200;
            t2_3.x = 130;
            t2_3.y = 140;
            t2_3.z = 130;

            #endregion track2

            //addting track 2 the scenario
            sc.tracks.Add(t2);


            #region track3
            //creating the third track for our test scenario
            Track t3 = new Track();

            //first plot within the third track
            Plot t3_1 = new Plot();
            t3_1.timestamp = DateTime.Now;
            t3_1.speed = 333;
            t3_1.x = 220;
            t3_1.y = 230;
            t3_1.z = 240;
            //add the plot to the track
            t2.plots.Add(t1_1);

            //second plot within the third track after 4 seonds
            Plot t3_2 = new Plot();
            t3_2.timestamp = DateTime.Now.AddSeconds(4);
            t3_2.speed = 500;
            t3_2.x = 225;
            t3_2.y = 225;
            t3_2.z = 235;


            //third plot within the third track after 8 seonds
            Plot t3_3 = new Plot();
            t3_3.timestamp = DateTime.Now.AddSeconds(8);
            t3_3.speed = 200;
            t3_3.x = 230;
            t3_3.y = 240;
            t3_3.z = 230;

            #endregion track3

            //addting track 3 the scenario
            sc.tracks.Add(t3);
        }

    }
}
