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
using System.Threading.Tasks;

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

        //store the next avilable track ID
        int avilableTrackID;

        //the thread for incrementing time (current server time)
        Thread timeThread;


        public MainSimulation()
        {
            #region Initilizing variables
            clients = new List<IClientCallbackInterface>();
            layeredScenarios = new Dictionary<int, Scenario>();
            currentServerTime = 0;
            mainScenario = new Scenario();
            avilableTrackID = 0;
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
                callback.notifyNewScenario(mainScenario);

            }
            catch (Exception e)
            {
                debugMessage("Failed to populateClient", e);

            }
        }

        public void createScenario()
        {
            try
            {
                //check if the client is in the callback list
                checkIfRegistered();

                mainScenario = new Scenario();

                //for testing pruposes only
                mainScenario = Test.populateScenarioBigger();

                avilableTrackID = mainScenario.tracks.Count;

                //sending the new scenario to the clients
                 //notify the other clients
                    clients.ForEach(
                 delegate(IClientCallbackInterface callback)
                 {
                     callback.notifyNewScenario(mainScenario);

                 });
                

            }
            catch (Exception e)
            {
                debugMessage("failed to create and send a scenario", e);
                throw new Exception("ATMS-MainSimulation-0001: Failed to create a new scenario");
            };
        }

        /**
         *  
         * the IServerInterface implementation of createnewtask
         * */
        public void createNewTrack(Track t)
        {
            //lets check if the client is registered
            checkIfRegistered();
            //check if the value incoming is Null
            if (t == null)
            {
                t = new Track();
            }

            //set an avilable track id to it
            t.trackID = avilableTrackID;

            //increment the track id
            avilableTrackID++;

            //adding the track to the mainscenario
            mainScenario.tracks.Add(t);

            //notify the clients of the newly added track
            clients.ForEach(
                delegate(IClientCallbackInterface callback)
                {
                    callback.notifyNewTrack(t);

                });

        }

        /**
         * sprint 6
         * */
        public void removeTrack(Track t)
        {
            //lets check if the client is registered
            checkIfRegistered();

            //check if null else we skip
            if (t != null)
            {
                //removing the track from our scenario, how we are sure we identify the same track is through our implementation of equals 
                mainScenario.tracks.Remove(t);

                //notify all the clients 
                clients.ForEach(
                 delegate(IClientCallbackInterface callback)
                 {
                     callback.notifyRemoveTrack(t);

                 });
            }
        }

        /**
         * sprint 6
         * */
        public void editTrack(Track t)
        {

            //lets check if the client is registered
            checkIfRegistered();
            //check if null else we skip
            if (t != null)
            {
                //finding the track to be changed
                Track trackToBeChanged = mainScenario.tracks.First(x => x.Equals(t));

                //check if we found something
                if (trackToBeChanged != null)
                {
                    //edit what we found
                    trackToBeChanged.edit(t);


                    //notify the other clients
                    clients.ForEach(
                 delegate(IClientCallbackInterface callback)
                 {
                     callback.notifyEditedTrack(t);

                 });
                }



            }
        }


        /**
         * 
         * Helper function to check if the callback client is allready in our list of clients
         * */
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
                    clients.ForEach(
              delegate(IClientCallbackInterface callback)
              {
                  callback.notifyNewScenario(mainScenario);

              });
                }
                catch (Exception e)
                {
                    //handle that the scenario is to big and can't be sent like this 
                    debugMessage("Failed to send new scenario", e);

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
                clients.ForEach(
              delegate(IClientCallbackInterface callback)
              {
                  callback.notifyTimeUpdate(currentServerTime);

              });
                


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
                debugMessage("Failed to handleClientCallback.", e);
            }
        }

        //handle  debugging message 
        public void debugMessage(string explanation, Exception e)
        {
            Debug.WriteLine("ATMS/" + this.GetType().Name + "- " + explanation);
            Debug.WriteLine("Excpetion" + e);
            Debug.WriteLine("Stacktrace:" + e.StackTrace);
        }
    }
}
