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
        private static List<IClientCallbackInterface> clients;

        //this is the main scenario
        private Scenario mainScenario;

        //declaring a dictionary to store the layered scenarios , the 
        private Dictionary<int, Scenario> layeredScenarios;

        //current server time in seconds
        int currentServerTime;
        //the thread for incrementing time (current server time)
        Thread timeThread;

        public MainSimulation()
        {
            #region Initilizing variables
            clients = new List<IClientCallbackInterface>();

            layeredScenarios = new Dictionary<int, Scenario>();

            currentServerTime = 0;
            #endregion
        }

        /*
         *  The implementation of the IServerInterface for the duplex contract
         * */
        #region IserverInterface

        public void playSimulation()
        {
            //notify all client we are playing
            //  clients.Select(x => x.Value).ToList().ForEach(c => c.notifyTimeUpdate(currentServerTime));

            //start the timeworker thread who is responsable for calinn the incremental time funciton depending on the radarinterval
            TimeWorker worker = new TimeWorker(this);
            timeThread = new Thread(worker.DoWork);
            timeThread.Start();
        }

        public void RegisterClient()
        {
            try
            {
                IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();
                clients.Add(callback);

                //for testing / debugging purposes - it freezez up upon startup
                if (mainScenario != null)
                {
                    ThreadPool.QueueUserWorkItem(a => { Thread.Sleep(5000); callback.notifyNewScenario(mainScenario); });
                }
            }
            catch (Exception)
            {
                throw new Exception("ATMS-MainSimulation-0001: Failed to register client");
            }
        }

        public void createScenario()
        {
            try
            {
                mainScenario = new Scenario();
                populateScenario(mainScenario);
                ThreadPool.QueueUserWorkItem(a => notifyClients());
            }
            catch (Exception)
            {
                throw new Exception("ATMS-MainSimulation-0001: Failed to create a new scenario");
            };
        }

        public bool checkIfRegistered()
        {
            if (clients.Contains(OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>()))
            {
                return true;
            }
            return false;
        }

        #endregion

        /*
         *  The implementation of the callbacks to the callbackinteerface in the duplex contract
         * */
        #region Communication with client

        //Thread to handle the callbacks
        private void handleClientCallbacks(Action a, IClientCallbackInterface client, List<IClientCallbackInterface> clientsList)
        {
            TimeoutWorker t = new TimeoutWorker();
            t.DoWork(a, client, clientsList);
        }

        public void notifyClients()
        {
            foreach (IClientCallbackInterface entry in clients)
            {
                //Handle the client callbacks, 1st argument is the function, 2nd is the client and 3rd is the client list
                handleClientCallbacks(() => { entry.notifyNewScenario(mainScenario); }, entry, clients);
            }
        }

        #endregion

        //TEST method for populating scenarios with test data
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
            t2.plots.Add(t2_1);

            //second plot within the second track after 4 seonds
            Plot t2_2 = new Plot();
            t2_2.timestamp = DateTime.Now.AddSeconds(4);
            t2_2.speed = 500;
            t2_2.x = 125;
            t2_2.y = 135;
            t2_2.z = 135;
            //add the plot to the track
            t2.plots.Add(t2_2);

            //third plot within the second track after 8 seonds
            Plot t2_3 = new Plot();
            t2_3.timestamp = DateTime.Now.AddSeconds(8);
            t2_3.speed = 200;
            t2_3.x = 130;
            t2_3.y = 140;
            t2_3.z = 130;
            //add the plot to the track
            t2.plots.Add(t2_3);

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
            t2.plots.Add(t3_1);

            //second plot within the third track after 4 seonds
            Plot t3_2 = new Plot();
            t3_2.timestamp = DateTime.Now.AddSeconds(4);
            t3_2.speed = 500;
            t3_2.x = 225;
            t3_2.y = 225;
            t3_2.z = 235;
            //add the plot to the track
            t2.plots.Add(t3_2);

            //third plot within the third track after 8 seonds
            Plot t3_3 = new Plot();
            t3_3.timestamp = DateTime.Now.AddSeconds(8);
            t3_3.speed = 200;
            t3_3.x = 230;
            t3_3.y = 240;
            t3_3.z = 230;
            //add the plot to the track
            t2.plots.Add(t3_3);
            #endregion track3

            //addting track 3 the scenario
            sc.tracks.Add(t3);
        }

        public void tickTock()
        {
            //incrementing the time by the value in the radarinterval
            currentServerTime += ATMS_Model.BuisnessLogicValues.radarInterval;

            //notifying listening clients of the update
            foreach (IClientCallbackInterface entry in clients)
            {
                handleClientCallbacks(() => { entry.notifyTimeUpdate(currentServerTime); }, entry, clients);
            }
        }
    }
}
