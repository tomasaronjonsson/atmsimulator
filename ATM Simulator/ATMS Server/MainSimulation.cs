using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using ATMS_Model;

//for debugging
using System.Diagnostics;

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
            mainScenario = new Scenario();
            #endregion
        }

        /*
         *  The implementation of the IServerInterface for the duplex contract
         * */
        #region IserverInterface

        public void playSimulation()
        {
            //check if the client is in the callback list
            checkIfRegistered();

            if (timeThread == null)
            {
                //start the timeworker thread who is responsable for calling the incremental time funciton depending on the radarinterval
                TimeWorker worker = new TimeWorker(this);
                timeThread = new Thread(worker.DoWork);
                timeThread.Start();
            }
        }

        public void populateClient()
        {
            try
            {
                checkIfRegistered();
                IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();

                //send the data 
                ThreadPool.QueueUserWorkItem(work => handleClientCallback(() => { callback.notifyNewScenario(mainScenario); }, callback));

            }
            catch (Exception e)
            {
                debugMessage(e);
                throw new Exception("ATMS-MainSimulation-0001: Failed to register client");

            }
        }

        public void createScenario()
        {
            try
            {
                //check if the client is in the callback list
                checkIfRegistered();

                mainScenario = new Scenario();
                populateScenarioBigger(mainScenario);
                //sending the new scenario to the clients
                ThreadPool.QueueUserWorkItem(a => sendNewScenario());

            }
            catch (Exception e)
            {
                debugMessage(e);
                throw new Exception("ATMS-MainSimulation-0001: Failed to create a new scenario");
            };
        }

        public void checkIfRegistered()
        {
            IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();

            if (!clients.Contains(callback))
            {
                clients.Add(callback);
            }
        }



        #endregion

        /*
         *  The implementation of the callbacks to the callbackinteerface in the duplex contract
         * */
        #region Communication with client



        public void sendNewScenario()
        {
            foreach (IClientCallbackInterface entry in clients)
            {
                try
                {
                    //Handle the client callbacks, 1st argument is the function, 2nd is the client and 3rd is the client list
                    ThreadPool.QueueUserWorkItem(work => handleClientCallback(() => { entry.notifyNewScenario(mainScenario); }, entry));
                }
                catch (Exception e)
                {
                    //handle that the scenario is to big and can't be sent like this 
                    debugMessage(e);

                }
            }
        }

        #endregion

        /*
         *  Implementation of the time incrementer and the notification of the clients
         * */
        public void tickTock()
        {
            //incrementing the time by the value in the radarinterval
            currentServerTime += ATMS_Model.BuisnessLogicValues.radarInterval;

            //notifying listening clients of the update
            foreach (IClientCallbackInterface entry in clients)
            {
                ThreadPool.QueueUserWorkItem(work => handleClientCallback(() => { entry.notifyTimeUpdate(currentServerTime); }, entry));

            }
        }

        public void handleClientCallback(Action action, IClientCallbackInterface client)
        {

            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                clients.Remove(client);
                debugMessage(e);
            }
        }
        #region test methods
        private void populateScenarioBigger(Scenario sc)
        {
            int tracks = 100;
            int plots = 1000;
   
            double baseLatitude = 55.850223;
            double baseLongitude = 9.829360;

            Random randGen = new Random();

            for (int i = 0; i < tracks; i++)
            {
                Track track = new Track();
                track.trackID = i;

                for (int a = 0; a < plots; a++)
                {
                    Plot plot = new Plot();
                    plot.timestamp = DateTime.Now.AddSeconds(a * BuisnessLogicValues.radarInterval);
                    plot.speed = a;
                    double l = randGen.NextDouble() * 0.01;
                 
                    double la = randGen.NextDouble() * 0.01;
                    

                    plot.latitude = baseLatitude + l;
                    plot.longitude = baseLongitude + la;
                    //todo
                    plot.altitude = a * 4;

                    track.plots.Add(plot);
                }
                sc.tracks.Add(track);
            }
        }


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
            t1_1.latitude = 20;
            t1_1.longitude = 30;
            t1_1.altitude = 40;
            //add the plot to the track
            t1.plots.Add(t1_1);

            //second plot within the first track after 4 seonds
            Plot t1_2 = new Plot();
            t1_2.timestamp = DateTime.Now.AddSeconds(4);
            t1_2.speed = 500;
            t1_2.latitude = 25;
            t1_2.longitude = 35;
            t1_2.altitude = 35;
            //add the plot to the track
            t1.plots.Add(t1_2);


            //third plot within the first track after 8 seonds
            Plot t1_3 = new Plot();
            t1_3.timestamp = DateTime.Now.AddSeconds(8);
            t1_3.speed = 500;
            t1_3.latitude = 30;
            t1_3.longitude = 40;
            t1_3.altitude = 30;
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
            t2_1.latitude = 120;
            t2_1.longitude = 130;
            t2_1.altitude = 140;
            //add the plot to the track
            t2.plots.Add(t2_1);

            //second plot within the second track after 4 seonds
            Plot t2_2 = new Plot();
            t2_2.timestamp = DateTime.Now.AddSeconds(4);
            t2_2.speed = 500;
            t2_2.latitude = 125;
            t2_2.longitude = 135;
            t2_2.altitude = 135;
            //add the plot to the track
            t2.plots.Add(t2_2);

            //third plot within the second track after 8 seonds
            Plot t2_3 = new Plot();
            t2_3.timestamp = DateTime.Now.AddSeconds(8);
            t2_3.speed = 200;
            t2_3.latitude = 130;
            t2_3.longitude = 140;
            t2_3.altitude = 130;
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
            t3_1.latitude = 220;
            t3_1.longitude = 230;
            t3_1.altitude = 240;
            //add the plot to the track
            t2.plots.Add(t3_1);

            //second plot within the third track after 4 seonds
            Plot t3_2 = new Plot();
            t3_2.timestamp = DateTime.Now.AddSeconds(4);
            t3_2.speed = 500;
            t3_2.latitude = 225;
            t3_2.longitude = 225;
            t3_2.altitude = 235;
            //add the plot to the track
            t2.plots.Add(t3_2);

            //third plot within the third track after 8 seonds
            Plot t3_3 = new Plot();
            t3_3.timestamp = DateTime.Now.AddSeconds(8);
            t3_3.speed = 200;
            t3_3.latitude = 230;
            t3_3.longitude = 240;
            t3_3.altitude = 230;
            //add the plot to the track
            t2.plots.Add(t3_3);
            #endregion track3

            //addting track 3 the scenario
            sc.tracks.Add(t3);
        }
        #endregion

        public void debugMessage(Exception e)
        {
            Debug.WriteLine("Excpetion" + e);
            Debug.WriteLine("Stacktrace:" + e.StackTrace);
        }
    }
}
